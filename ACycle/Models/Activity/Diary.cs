using Newtonsoft.Json;

namespace ACycle.Models
{
    public record class Diary : Entry
    {
        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("date_time")]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [JsonProperty("content")]
        public string Content { get; set; } = string.Empty;
    }
}
