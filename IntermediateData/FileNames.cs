using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using CheckContracts;
using JetBrains.Annotations;

namespace IntermediateData
{
    public static class FileNames
    {
        public static async Task SaveFileListAsync([NotNull] string destination, ImmutableList<string> files)
        {
            Validate.StringIsMeanful(destination, nameof(destination));

            var data = new FilesData(files.ToArray());

            await data.SaveObjectAsync(destination).ConfigureAwait(false);
        }
    }
}