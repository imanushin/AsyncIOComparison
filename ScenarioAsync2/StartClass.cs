using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IntermediateData;

namespace ScenarioAsync2
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
            var tasks = filesList.Select(CheckFileLengthAsync).ToArray();

            await Task.WhenAll(tasks).ConfigureAwait(false);

            return (int) tasks.Select(t => t.Result).Average();
        }

        private static async Task<int> CheckFileLengthAsync(string filePath)
        {
            using (var file = File.OpenRead(filePath))
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream).ConfigureAwait(false);

                    return (int) stream.Position;
                }
            }
        }
    }
}
