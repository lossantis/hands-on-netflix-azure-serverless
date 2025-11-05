using Newtonsoft.Json;

namespace HandsOnNetflixAzureServerless
{
    public class MovieResponse
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("year")]
        public string? Year { get; set; }

        [JsonProperty("video")]
        public string? Video { get; set; }

        [JsonProperty("thumb")]
        public string? Thumb { get; set; }
    }
}