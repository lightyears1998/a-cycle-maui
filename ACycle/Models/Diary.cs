namespace ACycle.Models
{
    public class Diary : EntryBasedModel
    {
        public override string EntryContentType { get; } = "diary";

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}
