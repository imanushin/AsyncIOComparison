using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckContracts;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace IntermediateData
{
    public sealed class FileNames
    {
        public static async Task SaveToFileAsync([NotNull] string destination, ImmutableList<string> files)
        {
            Validate.StringIsMeanful(destination, nameof(destination));

            var data = new FilesData(files.ToArray());

            using (var file = File.Open(destination, FileMode.Create))
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var writer = new StreamWriter(memoryStream))
                    {
                        var serializer = JsonSerializer.CreateDefault();

                        serializer.Serialize(writer, data);

                        await writer.FlushAsync().ConfigureAwait(false);

                        memoryStream.Seek(0, SeekOrigin.Begin);

                        await memoryStream.CopyToAsync(file).ConfigureAwait(false);
                    }
                }

                await file.FlushAsync().ConfigureAwait(false);
            }
        }
    }
}
