

using System;
using System.Collections.Immutable;
using System.Linq;

namespace TestsHost
{
    internal sealed class AggregatedValue
    {
        private const int IgnoredResults = 1; // these count of slow/fast results will be skipped finally

        public AggregatedValue(double min, double max, double avg)
        {
            Min = min;
            Max = max;
            Avg = avg;
        }

        public double Min { get; }

        public double Max { get; }

        public double Avg { get; }

        public static AggregatedValue Create(ImmutableList<double> values)
        {
            var clearedValues = values.Where(v => v > double.Epsilon).ToImmutableList();

            if (clearedValues.Count < 3)
            {
                return new AggregatedValue(0, 0, 0);
            }

            var sortedValues = clearedValues.Sort();

            var withoutSlowAndFast =
                sortedValues.Skip(IgnoredResults).Take(sortedValues.Count - IgnoredResults * 2).ToImmutableList();

            var avg = withoutSlowAndFast.Average();

            return new AggregatedValue(min: sortedValues.Min(), max: sortedValues.Max(), avg: avg);
        }
    }
}