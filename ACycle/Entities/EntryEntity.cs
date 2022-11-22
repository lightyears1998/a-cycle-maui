using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACycle.Entities
{
    [Table("entry")]
    public class EntryEntity
    {
        [PrimaryKey]
        [Column("uuid")]
        public string Uuid { set; get; } = Guid.NewGuid().ToString();

        [Column("removedAt")]
        public DateTime RemovedAt { set; get; }
    }
}
