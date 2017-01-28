using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CheckContracts;
using IntermediateData;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace TestsHost
{
    internal static class Program
    {
        private static readonly SimpleLayout NLogLayout = new SimpleLayout("${longdate}|${level:uppercase=true}|${logger}|${message}|${exception:format=ToString}");
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

#if DEBUG
        private const int AttemptsCount = 3;
#else
        private const int AttemptsCount = 7;
#endif

#if DEBUG
        private static readonly int FilesCount = 10000;
#else
        private static readonly int FilesCount = 10000;
#endif

        private static void InitLogging()
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ConsoleTarget("console") { Layout = NLogLayout };
            var fileTarget = new FileTarget("file") { FileName = "testhost_" + DateTime.UtcNow.Ticks + ".txt", Layout = NLogLayout };

            config.AddTarget(consoleTarget);
            config.AddTarget(fileTarget);

            config.AddRuleForAllLevels(consoleTarget);
            config.AddRuleForAllLevels(fileTarget);

            LogManager.Configuration = config;
        }

        public static int Main()
        {
            try
            {
                MainAsync().Wait();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unhandled exception");

                LogManager.Configuration = null;

                return -1;
            }

            LogManager.Configuration = null;

            return 0;
        }

        private static async Task MainAsync()
        {
            InitLogging();

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
                    Log.Info("Tests done");
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

            Log.Info($"Files to test: {filesToTest.Count}, min size - {minSize} bytes, max size - {maxSize}, average size - {avgSize}");

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

                WriteScenarioResults(aggregatedResult, scenario);
                await resultsStream.FlushAsync().ConfigureAwait(false);
            }

            await resultsStream.WriteLineAsync().ConfigureAwait(false);
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

        private static void WriteScenarioResults(MultipleExecutionResult testResult, string scenario)
        {
            if (testResult.WasFailed)
            {
                Log.Info($"{scenario} - some tests were failed");
            }
            else
            {
                var logOutput = new StringBuilder();
                logOutput.Append($"{scenario} - {testResult.ExecutionTime} secs; ");

                foreach (var aggregatedCounter in testResult.CounterValues)
                {
                    var name = aggregatedCounter.Key;
                    var min = aggregatedCounter.Value.Min;
                    var avg = aggregatedCounter.Value.Avg;
                    var max = aggregatedCounter.Value.Max;
                    var appendix = ProcessAnalyzer.GetAppendix(aggregatedCounter.Key);

                    logOutput.AppendLine($"{name} - {min}/{avg}/{max} {appendix} ");
                }

                Log.Info(logOutput);
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