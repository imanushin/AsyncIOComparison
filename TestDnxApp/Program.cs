using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TestDnxApp
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener()); 
            
            try
            {
                Console.WriteLine("Hello World");
                Console.WriteLine();
                Console.Read();                            
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
