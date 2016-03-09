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
            Trace.TraceInformation("Root folders: {0}; total files: {1}", string.Join(";", RootFolders), PreparedFiles.Count);

            return PreparedFiles
                .Where(fi => fi.Length >= minLength)
                .Take(maxCount)
                .ToImmutableList();
        }

        private static ImmutableList<string> GetRootFolderCandidates()
        {
            return new[] { "/System/Library", "%ProgramFiles%", "%windir%" }
            .Select(Environment.ExpandEnvironmentVariables)
            .ToImmutableList();
        }

        private static ImmutableList<FileInfo> GetFiles(string path)
        {
            var files = ImmutableList<FileInfo>.Empty.ToBuilder();

            try
            {
                files.AddRange(
                    Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly)
                        .AsParallel()
                        .Select(f => new FileInfo(f))
                        .Where(f => f.Exists)
                        .Where(CanBeOpened));

                foreach (var directory in Directory.GetDirectories(path))
                {
                    files.AddRange(GetFiles(directory));

                    if (files.Count > 1000000)
                    {
                        break;
                    }
                }
            }
            catch (UnauthorizedAccessException) { }

            return files.ToImmutable();
        }

        private static bool CanBeOpened(FileInfo fileInfo)
        {
            try
            {
                fileInfo.OpenRead().Dispose();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}