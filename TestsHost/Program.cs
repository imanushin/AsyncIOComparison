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

            using (var file = File.Open(outFile, FileMode.Create))
            {

                foreach (var sizeDegree in Enumerable.Range(0, 7))
                {
                    var size = (int)Math.Pow(10, sizeDegree);

                    await CheckFilesAsync(size).ConfigureAwait(false);
                }

                await file.FlushAsync().ConfigureAwait(false);
                await Console.Out.WriteLineAsync("Tests done").ConfigureAwait(false);
            }
        }

        private static async Task CheckFilesAsync(int minFileSize)
        {
            var filesToTest = FilesLookup.FindFiles(10000, minFileSize);

            var minSize = filesToTest.Select(f => f.Length).Min();
            var maxSize = filesToTest.Select(f => f.Length).Max();
            var avgSize = filesToTest.Select(f => f.Length).Average();

            await Console.Out.WriteLineAsync($"Files to test: {filesToTest.Count}, min size - {minSize} bytes, max size - {maxSize}, average size - {avgSize}").ConfigureAwait(false);
            await Console.Out.WriteLineAsync().ConfigureAwait(false);

            var exeFileName = Process.GetCurrentProcess().MainModule.FileName;
            var currentFolder = Path.GetDirectoryName(exeFileName);
            Validate.IsNotNull(currentFolder);
            var filesListFile = Path.Combine(currentFolder, "filesToTest.json");
            await
                FileNames.SaveFileListAsync(filesListFile, filesToTest.Select(fi => fi.FullName).ToImmutableList())
                    .ConfigureAwait(false);

            var resultsFile = Path.Combine(currentFolder, "results.json");

            var arguments = new Arguments(filesListFile, resultsFile);

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
                var testResult = CheckScenario(scenario, arguments);
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
                        Console.Out.WriteLineAsync($"{scenario} - {result.ExecutionTime.TotalSeconds} secs; cpu - {cpuData}, memory - {memData}")
                            .ConfigureAwait(false);
                }
            }

            await Console.Out.WriteLineAsync().ConfigureAwait(false);
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

                while (!process.HasExited)
                {
                    cpuValues.Add(theCpuCounter.NextValue());
                    memValues.Add(theMemCounter.NextValue());

                    Thread.Sleep(1000);
                }

                process.WaitForExit();

                ExitResult exitResult;

                switch (process.ExitCode)
                {
                    case 0:
                        exitResult = ExitResult.Ok;
                        break;

                    case 1:
                        exitResult = ExitResult.Ok;
                        break;


                    default:
                        exitResult = ExitResult.Ok;
                        break;
                }

                var data = exitResult == ExitResult.Ok
                    ? Serialization.LoadObjectAsync<ResultsData>(arguments.PathToResults).Result
                    : null;

                return new TestResult(exitResult, data, cpuValues.ToImmutableList(), memValues.ToImmutableList());
            }
        }
    }
}
