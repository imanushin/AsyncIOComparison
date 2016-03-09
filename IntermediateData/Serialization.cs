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
    public static class Serialization
    {
        public static async Task SaveObjectAsync([NotNull] this object data, string destination)
        {
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

        public static async Task<TObject> LoadObjectAsync<TObject>(string fileName)
        {
            using (var file = File.OpenRead(fileName))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream).ConfigureAwait(false);

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    using (var writer = new StreamReader(memoryStream))
                    {
                        var serializer = JsonSerializer.CreateDefault();

                        return (TObject)serializer.Deserialize(writer, typeof(TObject));
                    }
                }
            }
        }
    }
}
