using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CheckContracts;
using IntermediateData;

namespace TestsHost
{
    internal static class Program
    {
        public static int Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener()); 
            
            try
            {
                MainAsync().Wait();
            }   
            catch(Exception ex)
            {
                Trace.TraceError("Unhandled exception: {0}", ex);
                
                return -1;
            }   

            return 0;
        }

        private static async Task MainAsync()
        {
            var filesToTest = FilesLookup.FindFiles(100);

            await Console.Out.WriteLineAsync($"Files to test: {filesToTest.Count}").ConfigureAwait(false);
            await Console.Out.WriteLineAsync().ConfigureAwait(false);

            var exeFileName = Process.GetCurrentProcess().MainModule.FileName;
            var currentFolder = Path.GetDirectoryName(exeFileName);
            Validate.IsNotNull(currentFolder);
            var filesListFile = Path.Combine(currentFolder, "filesToTest.json");
            await FileNames.SaveToFileAsync(filesListFile,filesToTest.Select(fi => fi.FullName).ToImmutableList()).ConfigureAwait(false);

            await Console.Out.WriteLineAsync("Tests done");
        }
    }
}
