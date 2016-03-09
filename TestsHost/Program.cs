using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CheckContracts;
using IntermediateData;

namespace TestsHost
{
    internal static class Program
    {
        public static int Main(string[] args)
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
            foreach (var sizeDegree in Enumerable.Range(0, 7))
            {
                var size = (int)Math.Pow(10, sizeDegree);

                await CheckFilesAsync(size).ConfigureAwait(false);
            }

            await Console.Out.WriteLineAsync("Tests done").ConfigureAwait(false);
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

            var scenarios = ImmutableList.Create("ScenarioSyncAsParallel", "ScenarioReadAllAsParallel", "ScenarioAsync", "ScenarioAsync2",
                "ScenarioNewThread");

            CheckScenario("ScenarioAsync", arguments); //warm system io caches

            foreach (var scenario in scenarios)
            {
                var result = CheckScenario(scenario, arguments);

                await Console.Out.WriteLineAsync($"{scenario} - {result.ExecutionTime.TotalSeconds} secs").ConfigureAwait(false);
            }

            await Console.Out.WriteLineAsync().ConfigureAwait(false);
        }

        private static ResultsData CheckScenario(string scenarioName, Arguments arguments)
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

                process.WaitForExit();
            }

            return Serialization.LoadObjectAsync<ResultsData>(arguments.PathToResults).Result;
        }

    }
}
