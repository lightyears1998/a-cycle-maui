namespace ACycle.Models
{
    public record class Diary : Entry
    {
        public string Title { get; set; } = string.Empty;

        public DateTime DateTime { get; set; } = DateTime.Now;

        public string Content { get; set; } = string.Empty;
    }
}
