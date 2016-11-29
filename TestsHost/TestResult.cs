using System.Collections.Immutable;
using IntermediateData;

namespace TestsHost
{
    internal sealed class TestResult
    {
        public TestResult(ExitResult exitResult, ResultsData data, ImmutableDictionary<string, double> counterToValue)
        {
            ExitResult = exitResult;
            Data = data;
            CounterToValue = counterToValue;
        }

        public ExitResult ExitResult { get; }

        public ResultsData Data { get; }

        public ImmutableDictionary<string, double> CounterToValue { get; }
    }
}