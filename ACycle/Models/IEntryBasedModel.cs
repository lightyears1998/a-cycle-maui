using Newtonsoft.Json;
using ACycle.Entities;

namespace ACycle.Models
{
    public interface IEntryBasedModel : IModel
    {
        [JsonIgnore]
        public static abstract string EntryContentType { get; }

        [JsonIgnore]
        public EntryEntity? EntryEntity { set; get; }
    }
}
