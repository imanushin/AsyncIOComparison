using System;

namespace TestsHost
{
    internal sealed class MultipleExecutionResult
    {
        public MultipleExecutionResult(
            TimeSpan executionTime,
            float averageProcessorTime,
            float averageMemoryUsage, 
            bool wasFailed)
        {
            ExecutionTime = executionTime;
            AverageProcessorTime = averageProcessorTime;
            AverageMemoryUsage = averageMemoryUsage;
            WasFailed = wasFailed;
        }

        public TimeSpan ExecutionTime { get; }

        public float AverageProcessorTime { get; }

        public float AverageMemoryUsage { get; }

        public bool WasFailed { get; }
    }
}