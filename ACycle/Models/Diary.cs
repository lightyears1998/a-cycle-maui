namespace ACycle.Models
{
    public class Diary : EntryBasedModel
    {
        public static string EntryContentType { get; } = "diary";

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}
