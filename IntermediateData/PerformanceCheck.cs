using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntermediateData
{
    public static class PerformanceCheck
    {
        public static void CheckPerformance(Arguments arguments, Func<ImmutableList<string>, int> aggregateFunc)
        {
            var filesList = FileNames.LoadFilesAsync(arguments.PathToFilesList).Result;

            var timer = Stopwatch.StartNew();

            aggregateFunc(filesList);

            var result = new ResultsData(timer.Elapsed);

            result.SaveObjectAsync(arguments.PathToResults).Wait();
        }
    }
}
