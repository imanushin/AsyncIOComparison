using System;
using Newtonsoft.Json;

namespace IntermediateData
{
    [JsonObject]
    public sealed class ResultsData
    {
        public ResultsData(TimeSpan executionTime)
        {
            ExecutionTime = executionTime;
        }

        [JsonProperty]
        public TimeSpan ExecutionTime { get; private set; }
    }
}