namespace ACycle.Models
{
    public record class ActivityCategory : Entry
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Guid? ParentUuid { get; set; }

        public DateTime? ArchivedAt { get; set; }

        public bool IsArchived
        {
            get => ArchivedAt != null;
            set
            {
                if (value != IsArchived)
                {
                    if (value)
                    {
                        ArchivedAt = DateTime.Now;
                    }
                    else
                    {
                        ArchivedAt = null;
                    }
                }
            }
        }
    }
}
