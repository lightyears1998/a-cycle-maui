using ACycle.Entities;
using ACycle.Models;
using Newtonsoft.Json;

namespace ACycle.Models
{
    public class Diary : IEntryBasedModel
    {
        public static string EntryContentType { get; } = "diary";

        public EntryEntity? EntryEntity { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}
