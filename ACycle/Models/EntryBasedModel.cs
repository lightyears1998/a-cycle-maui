using ACycle.Entities;
using Newtonsoft.Json;

namespace ACycle.Models
{
    public abstract class EntryBasedModel : IModel
    {
        [JsonIgnore]
        public static string EntryContentType { get; } = string.Empty;

        [JsonIgnore]
        public EntryEntity? Entry = null;
    }
}
