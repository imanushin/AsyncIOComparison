using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using CheckContracts;
using NLog;

namespace TestsHost
{
    internal sealed class ProcessAnalyzer : IDisposable
    {
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        private static readonly string MainDiskName =
            Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)).Substring(0, 2);

        private static readonly ImmutableSortedSet<string> CounterCategories = PerformanceCounterCategory
                .GetCategories()
                .Select(c => c.CategoryName)
                .ToImmutableSortedSet(StringComparer.Ordinal);

        private static readonly string CounterCategoriesList = string.Join(", ", CounterCategories);

        private static readonly PerformanceCounterData[] AllCounters =
        {
            PerformanceCounterData.Create(".NET CLR Memory", "# Bytes in all Heaps", "Total .Net Memory", "Mb", 1e6, GetProcessName),
            PerformanceCounterData.Create(".NET CLR LocksAndThreads", "Current Queue Length", "Current lock Queue", "Length", 1d, GetProcessName),
            PerformanceCounterData.Create(".NET CLR LocksAndThreads", "# of current logical Threads", "Concurrent Threads", "", 1d, GetProcessName),
            PerformanceCounterData.Create("LogicalDisk", "% Disk Read Time", "Disk Read Time", "", 1d, GetActiveDiskName),
            PerformanceCounterData.Create("LogicalDisk", "Disk Read Bytes/sec", "Disk Read", "Kb / sec", 1e3, GetActiveDiskName),
            PerformanceCounterData.Create("LogicalDisk", "Current Disk Queue Length", "Current Disk Queue", "Length", 1d, GetActiveDiskName),
            PerformanceCounterData.Create("LogicalDisk", "Split IO/Sec", "Disk Queue Length", "Count", 1d, GetActiveDiskName),
            PerformanceCounterData.Create("Process", "IO Read Bytes/sec", "IO Read", "KBytes/sec", 1e3, GetProcessName),
            PerformanceCounterData.Create("Process", "Page Faults/sec", "Page Faults", "Faults/sec", 1d, GetProcessName),
            PerformanceCounterData.Create("Process", "Thread Count", "Threads", "Count", 1d, GetProcessName),
            PerformanceCounterData.Create("Process", "% Processor Time", "CPU Load", "%", 1d, GetProcessName),
            PerformanceCounterData.Create("Memory", "Available MBytes", "Available MBytes", "Mb", 1d, GetMemoryInstanceName),
        };

        private static readonly ImmutableDictionary<string, PerformanceCounterData> NameToCounter =
            AllCounters.ToImmutableDictionary(cn => cn.TableTitleName, cn => cn);

        private readonly Process _process;

        private readonly Dictionary<string, PerformanceCounter> _counters;
        private readonly Dictionary<string, List<long>> _values;

        public ProcessAnalyzer(Process process)
        {
            _process = process;
            _counters = AllCounters.ToDictionary(cn => cn.TableTitleName, cn => CreateCounter(cn, process));
            _values = _counters.Keys.ToDictionary(c => c, c => new List<long>());
        }

        private static string GetProcessName(Process process)
        {
            return process.ProcessName;
        }

        private static string GetActiveDiskName(Process process)
        {
            return MainDiskName;
        }

        private static string GetMemoryInstanceName(Process process)
        {
            return null;
        }

        private PerformanceCounter CreateCounter(PerformanceCounterData counterData, Process process)
        {
            Validate.ArgumentCondition(!process.HasExited, nameof(process), "Process {0} was exited", process.ProcessName);
            
            var categoryName = counterData.CounterCategory;

            Validate.ArgumentCondition(
                CounterCategories.Contains(categoryName), 
                nameof(counterData),
                "Unable to find counter category {0}. Available categories: {1}",
                categoryName,
                CounterCategoriesList);

            var instanceName = counterData.InstanceResolver(process);

            var counter = new PerformanceCounter
            {
                CategoryName = categoryName,
                CounterName = counterData.CounterName,
                InstanceName = instanceName
            };

            return counter;
        }

        public void Collect()
        {
            try
            {
                var collectedData = _counters.ToImmutableDictionary(
                    kv => kv.Key, 
                    kv => (long)(CollectCounterData(kv.Value) / NameToCounter[kv.Key].Multiplier));

                // here values were read successfully, so we can add all data, value read error should not prevent us
                foreach (var performanceCounter in collectedData)
                {
                    var counterValue = performanceCounter.Value;
                    var name = performanceCounter.Key;

                    _values[name].Add(counterValue);
                }
            }
            catch (Exception ex)
            {
                Log.Warn(ex, "Unable to collect data for process {0}", _process.ProcessName);
            }
        }

        private static long CollectCounterData(PerformanceCounter counter)
        {
            try
            {
                return counter.RawValue;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Unable to collect values from category '{counter.CategoryName}', name '{counter.CounterName}' and instance '{counter.InstanceName}'", ex);
            }
        }

        public ImmutableDictionary<string, double> ExtractAverageValues()
        {
            return _values.ToImmutableDictionary(kv => kv.Key, kv => kv.Value.Average());
        }

        public void Dispose()
        {
            foreach (var counter in _counters.Values)
            {
                counter.Dispose();
            }
        }

        public static ImmutableList<string> CounterNames => NameToCounter.Keys.ToImmutableList();

        public static string GetAppendix(string name)
        {
            return NameToCounter[name].Dimension;
        }
    }
}

