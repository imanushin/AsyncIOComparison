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
        public static int CheckPerformance(Arguments arguments, Func<ImmutableList<string>, int> aggregateFunc)
        {
            var filesList = FileNames.LoadFilesAsync(arguments.PathToFilesList).Result;

            var timer = Stopwatch.StartNew();

            try
            {
                aggregateFunc(filesList);
            }
            catch (OutOfMemoryException)
            {
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }

            var result = new ResultsData(timer.Elapsed);

            result.SaveObjectAsync(arguments.PathToResults).Wait();

            return 0;
        }
    }
}
