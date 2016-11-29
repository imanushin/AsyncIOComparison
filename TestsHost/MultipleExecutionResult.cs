

using System;
using System.Collections.Immutable;

namespace TestsHost
{
    internal sealed class MultipleExecutionResult
    {
        public MultipleExecutionResult(
            TimeSpan executionTime,
            ImmutableDictionary<string, AggregatedValue> counterValues,
            bool wasFailed)
        {
            ExecutionTime = executionTime;
            CounterValues = counterValues;
            WasFailed = wasFailed;
        }

        public TimeSpan ExecutionTime { get; }

        public ImmutableDictionary<string, AggregatedValue> CounterValues { get; }

        public bool WasFailed { get; }
    }
}

