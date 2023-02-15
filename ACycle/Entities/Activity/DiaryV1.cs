using SQLite;

namespace ACycle.Entities
{
    [Table("entry_diary")]
    public class DiaryV1 : Entry
    {
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("date_time")]
        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        [Column("content")]
        public string Content { get; set; } = string.Empty;
    }
}
