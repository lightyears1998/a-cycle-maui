namespace ACycle.Models
{
    public class Diary : EntryBasedModel
    {
        public override string EntryContentType { get; } = "diary";

        public string title { get; set; }

        public string description { get; set; }

        public string content { get; set; }
    }
}
