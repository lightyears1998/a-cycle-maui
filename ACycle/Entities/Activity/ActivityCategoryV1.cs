using SQLite;

namespace ACycle.Entities
{
    [Table("entry_activity_category")]
    public class ActivityCategoryV1 : Entry
    {
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("parent_uuid")]
        public Guid? ParentUuid { get; set; }

        [Column("archived_at")]
        public DateTime? ArchivedAt { get; set; }
    }
}
