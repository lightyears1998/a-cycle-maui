using Newtonsoft.Json;

namespace ACycle.Repositories.Entry
{
    public record class EntryMetadata
    {
        [JsonProperty("uuid")]
        public Guid Uuid { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("updatedBy")]
        public Guid UpdatedBy { get; set; }
    }
}
