using ACycle.Entities;
using Newtonsoft.Json;

namespace ACycle.Models
{
    public abstract class EntryBasedModel : IModel
    {
        [JsonIgnore]
        public abstract string EntryContentType { get; }

        [JsonIgnore]
        public EntryEntity? Entry = null;
    }
}
