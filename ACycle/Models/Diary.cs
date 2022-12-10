using ACycle.Entities;
using Newtonsoft.Json;

namespace ACycle.Models
{
    public class Diary : IEntryBasedModel
    {
        public static string EntryContentType { get; } = "diary";

        public EntryEntity? EntryEntity { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("content")]
        public string Content { get; set; } = string.Empty;
    }
}
