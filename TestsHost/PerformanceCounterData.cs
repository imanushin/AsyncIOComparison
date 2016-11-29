using System;
using System.Diagnostics;
using CheckContracts;
using JetBrains.Annotations;

namespace TestsHost
{
    internal sealed class PerformanceCounterData
    {
        public static PerformanceCounterData Create(
            [NotNull] string counterCategory, 
            [NotNull] string counterName, 
            [NotNull] string tableTitleName, 
            [NotNull] string dimension, 
            double multiplier,
            [NotNull] Func<Process, string> instanceResolver)
        {
            Validate.ArgumentIsNotNull(counterCategory, nameof(counterCategory));
            Validate.ArgumentIsNotNull(counterName, nameof(counterName));
            Validate.ArgumentIsNotNull(tableTitleName, nameof(tableTitleName));
            Validate.ArgumentIsNotNull(dimension, nameof(dimension));
            Validate.ArgumentIsNotNull(instanceResolver, nameof(instanceResolver));

            return new PerformanceCounterData()
            {
                CounterCategory = counterCategory,
                CounterName = counterName,
                TableTitleName = tableTitleName,
                Dimension = dimension,
                Multiplier = multiplier,
                InstanceResolver = instanceResolver,
            };
        }

        [NotNull]
        public string CounterCategory { get; private set; }

        [NotNull]
        public string CounterName { get; private set; }

        [NotNull]
        public string TableTitleName { get; private set; }

        [NotNull]
        public string Dimension { get; private set; }
        
        public double Multiplier { get; private set; }

        [NotNull]
        public Func<Process, string> InstanceResolver { get; private set; }
    }
}