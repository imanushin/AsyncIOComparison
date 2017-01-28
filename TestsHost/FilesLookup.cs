using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace TestsHost
{
    internal static class FilesLookup
    {
        private static readonly ImmutableList<string> RestrictedSubstrings =
            ImmutableList.Create("WINDOWS\\SoftwareDistribution", "WINDOWS\\System32\\igfxCoIn", "NlbMigPlugin", "msoobedui", "PowerWmiProvider");
        
        private static readonly ImmutableList<DirectoryInfo> RootFolders = GetRootFolderCandidates()
            .Where(Directory.Exists)
            .Select(dn => new DirectoryInfo(dn))
            .ToImmutableList();

        public static ImmutableList<FileInfo> FindFiles(int maxCount, int minLength = 0)
        {
            return RootFolders
                .SelectMany(f => GetFiles(f.FullName, minLength))
                .Take(maxCount)
                .ToImmutableList();
        }

        private static ImmutableList<string> GetRootFolderCandidates()
        {
            return new[] { "/System/Library", "%ProgramFiles%", "%windir%" }
            .Select(Environment.ExpandEnvironmentVariables)
            .ToImmutableList();
        }

        private static IEnumerable<FileInfo> GetFiles(string path, int minLength = 0)
        {
            foreach (var file in GetFilesSafe(path)
                    .AsParallel()
                    .Select(f => new FileInfo(f))
                    .Where(f => IsAllowedFile(f) && f.Length > minLength)
                    .Where(CanBeOpened))
            {
                yield return file;
            }

            foreach (var directory in GetDirectoriesSafe(path))
            {
                foreach (var file in GetFiles(directory, minLength))
                {
                    yield return file;
                }
            }
        }

        private static bool IsAllowedFile(FileInfo file)
        {
            return file.Exists && !RestrictedSubstrings.Any(subPath => file.FullName.IndexOf(subPath, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private static string[] GetDirectoriesSafe(string path)
        {
            try
            {
                return Directory.GetDirectories(path);
            }
            catch (Exception)
            {
                return new string[0];
            }
        }

        private static string[] GetFilesSafe(string path)
        {
            try
            {
                return Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
            }
            catch (Exception)
            {
                return new string[0];
            }
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