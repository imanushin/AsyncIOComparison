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
            var filesToTest = FilesLookup.FindFiles(1000);

            await Console.Out.WriteLineAsync($"Files to test: {filesToTest.Count}").ConfigureAwait(false);
            await Console.Out.WriteLineAsync().ConfigureAwait(false);

            var exeFileName = Process.GetCurrentProcess().MainModule.FileName;
            var currentFolder = Path.GetDirectoryName(exeFileName);
            Validate.IsNotNull(currentFolder);
            var filesListFile = Path.Combine(currentFolder, "filesToTest.json");
            await FileNames.SaveFileListAsync(filesListFile, filesToTest.Select(fi => fi.FullName).ToImmutableList()).ConfigureAwait(false);

            var resultsFile = Path.Combine(currentFolder, "results.json");

            var arguments = new Arguments(filesListFile, resultsFile);

            var scenarios = ImmutableList.Create("ScenarioAsync");

            foreach (var scenario in scenarios)
            {
                var result = CheckScenario("ScenarioAsync", arguments);

                await Console.Out.WriteLineAsync($"{scenario} - {result.ExecutionTime.TotalSeconds} secs").ConfigureAwait(false);
            }

            await Console.Out.WriteLineAsync("Tests done").ConfigureAwait(false);
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
