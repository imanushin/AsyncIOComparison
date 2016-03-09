using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TestComparer
{
    internal static class FilesLookup
    {
        public static FileInfo[] FindFiles(int maxCount)
        {
            var rootFolders = GetRootFolderCandidates().Where(Directory.Exists).Select(dn=> new DirectoryInfo(dn));
            
            Trace.TraceInformation("Root folders: {0}", string.Join(";", rootFolders));
            
            return rootFolders
                .SelectMany(rf=>rf.EnumerateFiles("*", SearchOption.AllDirectories))
                .Take(maxCount)
                .ToArray();
        }
        
        private static string[] GetRootFolderCandidates()
        {
            return new [] {"/System/Library", "%windir%"};
        }
    }
}