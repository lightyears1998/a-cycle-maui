﻿using SQLite;

namespace ACycle.Services.DatabaseMigration.Schemes.V0
{
    [Table("entry")]
    internal class EntryV0
    {
        [PrimaryKey]
        [Column("uuid")]
        public Guid Uuid { get; set; }

        [Column("contentType")]
        public string ContentType { get; set; } = string.Empty;

        [Column("content")]
        public string Content { get; set; } = string.Empty;

        [Column("createdAt")]
        public string CreatedAt { get; set; } = string.Empty;

        [Column("removedAt")]
        public string? RemovedAt { get; set; } = string.Empty;

        [Column("updatedAt")]
        public string UpdatedAt { get; set; } = string.Empty;

        [Column("updatedBy")]
        public Guid UpdatedBy { get; set; }
    }
}
