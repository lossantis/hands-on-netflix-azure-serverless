using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HandsOnNetflixAzureServerless
{
    public class MovieRequest
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("title")]
        public string Title { get; set; } = default!;

        [JsonProperty("year")]
        public string Year { get; set; } = default!;

        [JsonProperty("video")]
        public string Video { get; set; } = default!;

        [JsonProperty("thumb")]
        public string Thumb { get; set; } = default!;
    }
}