﻿namespace ACycle.Models
{
    public record class Entry
    {
        public Guid Uuid { set; get; } = Guid.NewGuid();

        public DateTime? CreatedAt { set; get; }

        public Guid? CreatedBy { set; get; }

        public DateTime UpdatedAt { set; get; } = DateTime.Now;

        public Guid UpdatedBy { set; get; } = Guid.Empty;

        public DateTime? RemovedAt { set; get; }

        public bool IsCreated => CreatedAt.HasValue;

        public bool IsRemoved => RemovedAt.HasValue;
    }
}
