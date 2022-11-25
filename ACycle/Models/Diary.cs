using ACycle.Models.Attributes;

namespace ACycle.Models
{
    [EntryBasedModel("diary")]
    public class Diary : IModel
    {
        public static string EntryContentType { get; } = "diary";

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}
