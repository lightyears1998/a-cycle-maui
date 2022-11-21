using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACycle.Entities
{
    public class EntryEntity
    {
        [PrimaryKey]
        public string Uuid { set; get; } = Guid.NewGuid().ToString();
    }
}
