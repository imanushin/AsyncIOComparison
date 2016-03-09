using CheckContracts;
using JetBrains.Annotations;

namespace IntermediateData
{
    public sealed class Arguments
    {
        public Arguments([NotNull] string pathToFilesList, [NotNull] string pathToResults)
        {
            Validate.ArgumentStringIsMeanful(pathToFilesList, nameof(pathToFilesList));
            Validate.ArgumentStringIsMeanful(pathToResults, nameof(pathToResults));

            PathToFilesList = pathToFilesList;
            PathToResults = pathToResults;
        }

        public string PathToFilesList { get; }

        public string PathToResults { get; }

        public string ToCommandLine()
        {
            return $"\"{PathToFilesList}\" \"{PathToResults}\"";
        }

        public static Arguments Parse(string[] args)
        {
            return new Arguments(args[0], args[1]);
        }
    }
}