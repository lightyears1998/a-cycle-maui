using SQLite;

namespace ACycle.Entities
{
    [Table("entry_ledger_account")]
    public class AccountV1 : Entry
    {
        [Column("name")]
        public string Name { set; get; } = string.Empty;

        [Column("description")]
        public string Description { set; get; } = string.Empty;
    }
}
