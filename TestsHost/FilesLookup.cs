using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TestsHost
{
    internal static class FilesLookup
    {
        private static readonly ImmutableList<DirectoryInfo> RootFolders = GetRootFolderCandidates()
            .Where(Directory.Exists)
            .Select(dn => new DirectoryInfo(dn))
            .ToImmutableList();

        private static readonly ImmutableList<FileInfo> PreparedFiles =
            RootFolders.SelectMany(f => GetFiles(f.FullName)).ToImmutableList();

        public static ImmutableList<FileInfo> FindFiles(int maxCount, int minLength = 0)
        {
            Trace.TraceInformation("Root folders: {0}", string.Join(";", RootFolders));

            return PreparedFiles
                .Where(fi => fi.Length >= minLength)
                .Take(maxCount)
                .ToImmutableList();
        }

        private static ImmutableList<string> GetRootFolderCandidates()
        {
            return new[] { "/System/Library", "%ProgramFiles%" }
            .Select(Environment.ExpandEnvironmentVariables)
            .ToImmutableList();
        }

        private static ImmutableList<FileInfo> GetFiles(string path)
        {
            var files = ImmutableList<FileInfo>.Empty.ToBuilder();

            try
            {
                files.AddRange(Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Select(f => new FileInfo(f)));
                foreach (var directory in Directory.GetDirectories(path))
                {
                    files.AddRange(GetFiles(directory));
                }
            }
            catch (UnauthorizedAccessException) { }

            return files.ToImmutable();
        }
    }
}