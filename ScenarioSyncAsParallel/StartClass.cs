using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IntermediateData;

namespace ScenarioSyncAsParallel
{
    internal static class StartClass
    {
        private static int Main(string[] args)
        {
            var arguments = Arguments.Parse(args);

            PerformanceCheck.CheckPerformance(arguments, ProcessFiles);

            return 0;
        }

        private static int ProcessFiles(ImmutableList<string> filesList)
        {
            return (int)filesList.AsParallel().Select(CheckFileLength).Average();
        }

        private static int CheckFileLength(string filePath)
        {
            using (var file = File.OpenRead(filePath))
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);

                    return (int)stream.Position;
                }
            }
        }
    }
}
