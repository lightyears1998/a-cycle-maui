namespace ACycle.Models
{
    public record class Transaction : Entry
    {
        public TransactionKind Kind { set; get; } = TransactionKind.Relative;

        public string Summary { set; get; } = string.Empty;

        public string Details { set; get; } = string.Empty;
    }

    public enum TransactionKind
    {
        Absolute = 0,
        Relative = 1
    }
}
