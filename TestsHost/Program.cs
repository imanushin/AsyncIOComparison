﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CheckContracts;
using IntermediateData;

namespace TestsHost
{
    internal static class Program
    {
        private const int AttemptsCount = 5;
        private static readonly int FilesCount = 10000;

        public static int Main()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            try
            {
                MainAsync().Wait();
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unhandled exception: {0}", ex);

                return -1;
            }

            return 0;
        }

        private static async Task MainAsync()
        {
            var outDir = Environment.CurrentDirectory;
            var outFile = Path.Combine(outDir, "results_") + DateTime.UtcNow.Ticks + ".md";

            using (var resultsFile = File.Open(outFile, FileMode.Create))
            {
                using (var resultsStream = new StreamWriter(resultsFile))
                {
                    await resultsStream.WriteLineAsync($"**Check {FilesCount} files to read**").ConfigureAwait(false);
                    await resultsStream.WriteLineAsync("").ConfigureAwait(false);

                    foreach (var sizeDegree in new[] { 0, 3, 5, 6, 7, 8, 9 })
                    {
                        var size = (int)Math.Pow(10, sizeDegree);

                        await CheckFilesAsync(size, resultsStream).ConfigureAwait(false);
                    }

                    await resultsStream.FlushAsync().ConfigureAwait(false);
                    await resultsFile.FlushAsync().ConfigureAwait(false);
                    await Console.Out.WriteLineAsync("Tests done").ConfigureAwait(false);
                }
            }
        }

        private static async Task CheckFilesAsync(int minFileSize, StreamWriter resultsStream)
        {
            var filesToTest = FilesLookup.FindFiles(FilesCount, minFileSize);

            if (!filesToTest.Any())
            {
                return;
            }

            var minSize = filesToTest.Select(f => f.Length).Min();
            var maxSize = filesToTest.Select(f => f.Length).Max();
            var avgSize = filesToTest.Select(f => f.Length).Average();

            await Console.Out.WriteLineAsync($"Files to test: {filesToTest.Count}, min size - {minSize} bytes, max size - {maxSize}, average size - {avgSize}").ConfigureAwait(false);
            await Console.Out.WriteLineAsync().ConfigureAwait(false);

            await resultsStream.WriteLineAsync($"*Min size (bytes): {minSize} bytes, max size (bytes): {maxSize}, average size (bytes): {avgSize}*").ConfigureAwait(false);
            await resultsStream.WriteLineAsync().ConfigureAwait(false);

            await resultsStream.WriteAsync("| Scenario | Time ").ConfigureAwait(false);

            foreach (var counterName in ProcessAnalyzer.CounterNames)
            {
                await resultsStream.WriteAsync($"| {counterName} {ProcessAnalyzer.GetAppendix(counterName)} ").ConfigureAwait(false);
            }

            await resultsStream.WriteLineAsync("| Was failed |").ConfigureAwait(false);

            await resultsStream.WriteLineAsync("| -------- | -------- | -------- | -------- | -------- |").ConfigureAwait(false);

            var exeFileName = Process.GetCurrentProcess().MainModule.FileName;
            var currentFolder = Path.GetDirectoryName(exeFileName);
            Validate.IsNotNull(currentFolder);
            var filesListFile = Path.Combine(currentFolder, "filesToTest.json");

            await
                FileNames.SaveFileListAsync(filesListFile, filesToTest.Select(fi => fi.FullName).ToImmutableList())
                    .ConfigureAwait(false);

            var testResultsFilePath = Path.Combine(currentFolder, "results.json");

            var arguments = new Arguments(filesListFile, testResultsFilePath);

            var scenarios = ImmutableList.Create(
                "ScenarioAsyncWithMaxParallelCount4",
                "ScenarioAsyncWithMaxParallelCount8",
                "ScenarioAsyncWithMaxParallelCount16",
                "ScenarioAsyncWithMaxParallelCount32",
                "ScenarioAsyncWithMaxParallelCount64",
                "ScenarioAsyncWithMaxParallelCount128",
                "ScenarioAsyncWithMaxParallelCount256",
                "ScenarioSyncAsParallel",
                "ScenarioReadAllAsParallel",
                "ScenarioAsync",
                "ScenarioAsync2",
                "ScenarioNewThread");

            CheckScenario("ScenarioAsync", arguments); //warm system io caches

            foreach (var scenario in scenarios)
            {
                var multipleRunResults = Enumerable.Repeat(true, AttemptsCount).Select(_ => CheckScenario(scenario, arguments)).ToImmutableList();

                var aggregatedResult = AggregateResult(multipleRunResults);
                var counterValues = string.Join(" | ",
                    aggregatedResult.CounterValues.Values.Select(d => d.Avg.ToString("F")));

                await resultsStream.WriteLineAsync($"| {scenario} | {aggregatedResult.ExecutionTime} | {counterValues} | {aggregatedResult.WasFailed} |").ConfigureAwait(false);

                await WriteScenarioResultsAsync(aggregatedResult, scenario).ConfigureAwait(false);
                await resultsStream.FlushAsync().ConfigureAwait(false);
            }

            await resultsStream.WriteLineAsync().ConfigureAwait(false);

            await Console.Out.WriteLineAsync().ConfigureAwait(false);
        }

        private static MultipleExecutionResult AggregateResult(ImmutableList<TestResult> multipleRunResults)
        {
            if (multipleRunResults.Any(r => r.ExitResult != ExitResult.Ok))
            {
                return new MultipleExecutionResult(TimeSpan.Zero, ImmutableDictionary<string, AggregatedValue>.Empty, true);
            }

            var averageTime =
                TimeSpan.FromMilliseconds(
                    AggregatedValue.Create(
                        multipleRunResults.Select(r => (double)r.Data.ExecutionTime.Milliseconds).ToImmutableList()).Avg);

            var aggregatedCounters = ProcessAnalyzer.CounterNames.ToImmutableDictionary(
                cn => cn,
                cn => multipleRunResults.Select(r => r.CounterToValue[cn]).ToImmutableList());

            var medianCounters = GetMedianValues(aggregatedCounters);

            return new MultipleExecutionResult(averageTime, medianCounters, false);
        }

        private static ImmutableDictionary<string, AggregatedValue> GetMedianValues(ImmutableDictionary<string, ImmutableList<double>> multipleRun)
        {
            return multipleRun.ToImmutableDictionary(kv => kv.Key, kv => AggregatedValue.Create(kv.Value));
        }

        private static async Task WriteScenarioResultsAsync(MultipleExecutionResult testResult, string scenario)
        {
            if (testResult.WasFailed)
            {
                await Console.Out.WriteLineAsync($"{scenario} - some tests were failed").ConfigureAwait(false);
            }
            else
            {
                await Console.Out.WriteAsync($"{scenario} - {testResult.ExecutionTime} secs; ").ConfigureAwait(false);

                foreach (var aggregatedCounter in testResult.CounterValues)
                {
                    var name = aggregatedCounter.Key;
                    var min = aggregatedCounter.Value.Min;
                    var avg = aggregatedCounter.Value.Avg;
                    var max = aggregatedCounter.Value.Max;
                    var appendix = ProcessAnalyzer.GetAppendix(aggregatedCounter.Key);

                    await Console.Out.WriteAsync($"{name} - {min}/{avg}/{max} {appendix} ").ConfigureAwait(false);
                }

                await Console.Out.WriteLineAsync().ConfigureAwait(false);
            }
        }

        private static TestResult CheckScenario(string scenarioName, Arguments arguments)
        {
            var currentFolder = Path.GetDirectoryName(arguments.PathToFilesList);
            Validate.StringIsMeanful(currentFolder);

            if (File.Exists(arguments.PathToResults))
            {
                File.Delete(arguments.PathToResults);
            }

            var exeFile = Path.Combine(currentFolder, scenarioName) + ".exe";

            Validate.Condition(File.Exists(exeFile));

            using (var process = Process.Start(exeFile, arguments.ToCommandLine()))
            {
                Validate.IsNotNull(process);

                using (var processAnalyzer = new ProcessAnalyzer(process))
                {
                    Thread.Sleep(500);

                    while (!process.HasExited)
                    {
                        processAnalyzer.Collect();

                        Thread.Sleep(1000);
                    }

                    process.WaitForExit();

                    var exitResult = GetExitResult(arguments, process);

                    var data = exitResult == ExitResult.Ok
                        ? Serialization.LoadObjectAsync<ResultsData>(arguments.PathToResults).Result
                        : null;

                    return new TestResult(exitResult, data, processAnalyzer.ExtractAverageValues());
                }
            }
        }

        private static ExitResult GetExitResult(Arguments arguments, Process process)
        {
            switch (process.ExitCode)
            {
                case 0:
                    break;

                case 1:
                    return ExitResult.OutOfMemory;


                default:
                    return ExitResult.UnknownException;
            }

            var fileExists = File.Exists(arguments.PathToResults);

            return !fileExists ? ExitResult.UnknownException : ExitResult.Ok;
        }
    }
}