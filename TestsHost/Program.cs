using System;
using System.Diagnostics;

namespace TestsHost
{
    internal static class Program
    {
        public static int Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener()); 
            
            try
            {
                var filesToTest = FilesLookup.FindFiles(100);
                               
                Console.WriteLine("Files to test: {0}", filesToTest.Length);
                Console.WriteLine();
            }   
            catch(Exception ex)
            {
                Trace.TraceError("Unhandled exception: {0}", ex);
                
                return -1;
            }   

            return 0;
        }
    }
}
