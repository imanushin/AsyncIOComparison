using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntermediateData
{
    public static class PerformanceCheck
    {
        private static readonly string OutFileName = $"{Process.GetCurrentProcess().ProcessName}_{DateTime.UtcNow.Ticks}_failure_data.txt";

        public static int CheckPerformance(Arguments arguments, Func<ImmutableList<string>, int> aggregateFunc)
        {
            var filesList = FileNames.LoadFilesAsync(arguments.PathToFilesList).Result;

            var timer = Stopwatch.StartNew();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                aggregateFunc(filesList);

                var result = new ResultsData(timer.Elapsed);

                result.SaveObjectAsync(arguments.PathToResults).Wait();
            }
            catch (OutOfMemoryException ex)
            {
                GC.Collect();

                File.AppendAllText(OutFileName, ex + Environment.NewLine);

                return 1;
            }
            catch (Exception ex)
            {
                File.AppendAllText(OutFileName, ex + Environment.NewLine);

                return -1;
            }

            return 0;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            File.AppendAllText(OutFileName, e.ExceptionObject + Environment.NewLine);

            Environment.Exit(-1);
        }
    }
}
