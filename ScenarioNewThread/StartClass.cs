using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IntermediateData;

namespace ScenarioNewThread
{
    internal static class StartClass
    {
        private static int Main(string[] args)
        {
            var arguments = Arguments.Parse(args);

            return PerformanceCheck.CheckPerformance(arguments, ProcessFiles);
        }

        private static int ProcessFiles(ImmutableList<string> filesList)
        {
            return ProcessFilesAsync(filesList).Result;
        }

        private static async Task<int> ProcessFilesAsync(ImmutableList<string> filesList)
        {
            var tasks = filesList.AsParallel().Select(CheckFileLengthAsync).ToArray();

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return (int)tasks.Select(t => t.Result).Average();
        }

        private static Task<int> CheckFileLengthAsync(string filePath)
        {
            var tcs = new TaskCompletionSource<int>();

            var newThread = new Thread(() =>
            {
                using (var file = File.OpenRead(filePath))
                {
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);

                        tcs.SetResult((int)stream.Position);
                    }
                }
            });

            newThread.Start();

            return tcs.Task;
        }
    }
}
