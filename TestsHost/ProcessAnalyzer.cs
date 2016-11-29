using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace TestsHost
{
    internal sealed class ProcessAnalyzer : IDisposable
    {
        private static readonly Tuple<string, string, string, string, double>[] AllCounters =
        {
            Tuple.Create(".NET CLR Memory", "# Bytes in all Heaps", "Total .Net Memory", "Mb", 1e6)
        };
        /*
         * 
         ".NET CLR Memory" "# Bytes in all Heaps"
".NET CLR LocksAndThreads" "Concurent Queue Length"
".NET CLR LocksAndThreads" "# of current logical Threads"
"LogicalDisc" "# Disk Read Time"
"LogicalDisc" "Disk Read Bytes/sec"
"LogicalDisc" "Current Disk Queue Length"
"LogicalDisc" "Split IO/Sec"
"Process" "IO Read Bytes/sec"
"Process" "Page Faults/sec"
"Process" "Thread Count"
"Process" "% Processor Time"
"Memory" "Available MBytes"
         */
        private static readonly ImmutableDictionary<string, string> CounterToAppendix =
            AllCounters.ToImmutableDictionary(cn => cn.Item3, cn => cn.Item4);

        private readonly Dictionary<string, PerformanceCounter> _counters;
        private readonly Dictionary<string, List<long>> _values;

        public ProcessAnalyzer(Process process)
        {
            _counters = AllCounters.ToDictionary(cn => cn.Item3, cn => CreateCounter(cn.Item1, cn.Item2, process));
            _values = _counters.Keys.ToDictionary(c => c, c => new List<long>());
        }

        private PerformanceCounter CreateCounter(string categoryName, string counterName, Process process)
        {
            var counter = new PerformanceCounter
            {
                CategoryName = categoryName,
                CounterName = counterName,
                InstanceName = process.ProcessName
            };

            return counter;
        }

        public void Collect()
        {
            foreach (var performanceCounter in _counters)
            {
                var counter = performanceCounter.Value;
                var name = performanceCounter.Key;

                _values[name].Add(counter.RawValue);
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

        public static ImmutableList<string> CounterNames => AllCounters.Select(cn => cn.Item3).ToImmutableList();

        public static string GetAppendix(string name)
        {
            return CounterToAppendix[name];
        }
    }
}

