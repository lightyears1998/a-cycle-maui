using ACycle.Entities;
using ACycle.Repositories;
using Newtonsoft.Json;

namespace ACycle.Models
{
    public class Diary : EntryBasedModel
    {
        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("date")]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [JsonProperty("content")]
        public string Content { get; set; } = string.Empty;
    }
}
