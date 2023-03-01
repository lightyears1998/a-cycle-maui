using Newtonsoft.Json;

namespace ACycle.Models
{
    public record class ActivityCategory : Entry
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("parent_uuid")]
        public Guid? ParentUuid { get; set; }

        [JsonProperty("archived_at")]
        public DateTime? ArchivedAt { get; set; }

        [JsonIgnore]
        public bool IsArchived
        {
            get => ArchivedAt != null;
            set
            {
                if (value != IsArchived)
                {
                    if (value)
                    {
                        ArchivedAt = DateTime.Now;
                    }
                    else
                    {
                        ArchivedAt = null;
                    }
                }
            }
        }
    }
}
