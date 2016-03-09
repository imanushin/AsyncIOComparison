using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IntermediateData
{
    [JsonObject]
    internal sealed class FilesData
    {
        public FilesData(string[] files)
        {
            Files = files;
        }

        [JsonProperty]
        public string[] Files { get; private set; }

    }
}