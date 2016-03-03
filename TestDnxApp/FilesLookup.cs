
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TestDnxApp
{
    internal static class FilesLookup
    {
        public static FileInfo[] FindFiles(int maxCount)
        {
            var rootFolder = new DirectoryInfo("/System/Library");
            
            Trace.TraceInformation("Root folder: {0}", rootFolder);
            
            return rootFolder.EnumerateFiles("*", SearchOption.AllDirectories).Take(maxCount).ToArray();
        }
    }
}