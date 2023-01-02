using ACycle.Repositories;
using Newtonsoft.Json;

namespace ACycle.Models
{
    public class EntryBasedModel : IModel
    {
        [JsonIgnore]
        public EntryMetadata? EntryMetadata { set; get; }
    }
}
