using ACycle.Repositories;
using Newtonsoft.Json;

namespace ACycle.Models
{
    public class EntryBasedModel : IModel
    {
        [JsonIgnore]
        public Guid Uuid = Guid.NewGuid();

        [JsonIgnore]
        public EntryMetadata EntryMetadata = new();

        [JsonIgnore]
        public bool IsCreated => EntryMetadata.IsCreated;

        [JsonIgnore]
        public bool IsRemoved => EntryMetadata.IsRemoved;
    }
}
