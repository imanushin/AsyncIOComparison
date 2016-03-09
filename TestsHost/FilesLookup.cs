using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TestsHost
{
    internal static class FilesLookup
    {
        public static ImmutableList<FileInfo> FindFiles(int maxCount, int minLength = 0)
        {
            var rootFolders = GetRootFolderCandidates()
                .Where(Directory.Exists)
                .Select(dn => new DirectoryInfo(dn))
                .ToImmutableList();

            Trace.TraceInformation("Root folders: {0}", string.Join(";", rootFolders));

            return rootFolders
                .SelectMany(rf => rf.EnumerateFiles("*", SearchOption.AllDirectories).Where(fi => fi.Length > minLength))
                .Take(maxCount)
                .ToImmutableList();
        }

        private static ImmutableList<string> GetRootFolderCandidates()
        {
            return new[] { "/System/Library", "%ProgramFiles%" }
            .Select(Environment.ExpandEnvironmentVariables)
            .ToImmutableList();
        }
    }
}