using System.Collections.Immutable;
using System.IO;
using System.Linq;
using IntermediateData;

namespace ScenarioReadAllAsParallel
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
            return (int)filesList.AsParallel().Select(CheckFileLength).Average();
        }

        private static int CheckFileLength(string filePath)
        {
            return File.ReadAllBytes(filePath).Length;
        }
    }
}
