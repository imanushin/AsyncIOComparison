using System;
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
        private static readonly int FilesCount = 10000;
        private const int AttemptsCount = 5;

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

            await resultsStream.WriteLineAsync("| Scenario | Time | CPU usage (%) | Memory usage (Mb) | Was failed |").ConfigureAwait(false);
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
                var testResult = CheckScenario(scenario, arguments);

                await resultsStream.WriteLineAsync($"| {scenario} | {aggregatedResult.ExecutionTime} | {aggregatedResult.AverageProcessorTime} | {aggregatedResult.AverageMemoryUsage / 10000000} | {aggregatedResult.WasFailed} |").ConfigureAwait(false);

                await WriteScenarioResultsAsync(testResult, scenario).ConfigureAwait(false);
            }

            await resultsStream.WriteLineAsync().ConfigureAwait(false);

            await Console.Out.WriteLineAsync().ConfigureAwait(false);
        }

        private static MultipleExecutionResult AggregateResult(ImmutableList<TestResult> multipleRunResults)
        {
            if (multipleRunResults.Any(r => r.ExitResult != ExitResult.Ok))
            {
                return new MultipleExecutionResult(TimeSpan.Zero, 0, 0, true);
            }

            var averageTime = TimeSpan.FromMilliseconds(multipleRunResults.Average(r => r.Data.ExecutionTime.TotalMilliseconds));
            var averageCpu = multipleRunResults.Average(r => r.ProcessorTime.Average());
            var averageMem = multipleRunResults.Average(r => r.MemoryUsage.Average());

            return new MultipleExecutionResult(averageTime, averageCpu, averageMem, false);
        }

        private static async Task WriteScenarioResultsAsync(TestResult testResult, string scenario)
        {
            var result = testResult.Data;
            var cpuData = GetMinAvgMax(testResult.ProcessorTime);
            var memData = GetMinAvgMax(testResult.MemoryUsage);

            if (testResult.ExitResult != ExitResult.Ok)
            {
                await
                    Console.Out.WriteLineAsync($"{scenario} - failed with {testResult.ExitResult} error")
                        .ConfigureAwait(false);
            }
            else
            {
                await
                    Console.Out.WriteLineAsync(
                        $"{scenario} - {result.ExecutionTime.TotalSeconds} secs; cpu - {cpuData}, memory - {memData}")
                        .ConfigureAwait(false);
            }
        }

        private static string GetMinAvgMax(ImmutableList<float> data)
        {
            var min = data.Min();
            var max = data.Max();
            var avg = data.Average();

            return $"{min}/{avg}/{max}";
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

                var theCpuCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
                var theMemCounter = new PerformanceCounter("Process", "Working Set", process.ProcessName);

                var cpuValues = new List<float>();
                var memValues = new List<float>();

                Thread.Sleep(500);

                while (!process.HasExited)
                {
                    cpuValues.Add(theCpuCounter.NextValue());
                    memValues.Add(theMemCounter.NextValue());

                    Thread.Sleep(1000);
                }

                process.WaitForExit();

                var exitResult = GetExitResult(arguments, process);

                var data = exitResult == ExitResult.Ok
                    ? Serialization.LoadObjectAsync<ResultsData>(arguments.PathToResults).Result
                    : null;

                return new TestResult(exitResult, data, cpuValues.ToImmutableList(), memValues.ToImmutableList());
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
