using ACycle.Entities;
using Newtonsoft.Json;

namespace ACycle.Repositories.Entry
{
    public class EntryContainer : IEntryComparable
    {
        [JsonProperty("uuid")]
        public Guid Uuid { set; get; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { set; get; }

        [JsonProperty("createdBy")]
        public Guid? CreatedBy { set; get; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { set; get; } = DateTime.Now;

        [JsonProperty("updatedBy")]
        public Guid UpdatedBy { set; get; } = Guid.Empty;

        [JsonProperty("removedAt")]
        public DateTime? RemovedAt { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; } = string.Empty;

        [JsonProperty("content")]
        public string Content { get; set; } = string.Empty;
    }
}
