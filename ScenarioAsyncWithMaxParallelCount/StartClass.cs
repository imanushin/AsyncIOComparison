using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using IntermediateData;

namespace ScenarioAsync2
{
    internal static class StartClass
    {
#if MAX_PARALLEL_4
        private const int MaxParallelReads = 4;
#else
#if MAX_PARALLEL_8
        private const int MaxParallelReads = 8;
#else
#if MAX_PARALLEL_16
        private const int MaxParallelReads = 16;
#else
#if MAX_PARALLEL_24
        private const int MaxParallelReads = 24;
#else
#if MAX_PARALLEL_32
        private const int MaxParallelReads = 32;
#else
#if MAX_PARALLEL_64
        private const int MaxParallelReads = 64;
#else
#if MAX_PARALLEL_128
        private const int MaxParallelReads = 128;
#else
        private const int MaxParallelReads = 256;
#endif
#endif
#endif
#endif
#endif
#endif
#endif

        private static int Main(string[] args)
        {
            Console.Out.WriteLine("Parallel reads - {0}", MaxParallelReads);

            var arguments = Arguments.Parse(args);

            return PerformanceCheck.CheckPerformance(arguments, ProcessFiles);
        }

        private static int ProcessFiles(ImmutableList<string> filesList)
        {
            return ProcessFilesAsync(filesList).Result;
        }

        private static async Task<int> ProcessFilesAsync(ImmutableList<string> filesList)
        {
            var workerBlock = new ActionBlock<string>(
               CheckFileLengthAsync,
               new ExecutionDataflowBlockOptions
               {
                   MaxDegreeOfParallelism = MaxParallelReads
               });

            foreach (var file in filesList)
            {
                workerBlock.Post(file);
            }

            workerBlock.Complete();

            await workerBlock.Completion.ConfigureAwait(false);

            return 1;
        }

        private static async Task<int> CheckFileLengthAsync(string filePath)
        {
            using (var file = File.OpenRead(filePath))
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream).ConfigureAwait(false);

                    return (int)stream.Position;
                }
            }
        }
    }
}
