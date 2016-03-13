using System.Collections.Immutable;
using IntermediateData;

namespace TestsHost
{
    internal sealed class TestResult
    {
        public TestResult(ExitResult exitResult, ResultsData data, ImmutableList<float> processorTime, ImmutableList<float> memoryUsage)
        {
            ExitResult = exitResult;
            Data = data;
            ProcessorTime = processorTime;
            MemoryUsage = memoryUsage;
        }

        public ExitResult ExitResult { get; }

        public ResultsData Data { get; }

        public ImmutableList<float> ProcessorTime { get; }

        public ImmutableList<float> MemoryUsage { get; }
    }
}