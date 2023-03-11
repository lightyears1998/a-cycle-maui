namespace ACycle.Models
{
    public record class Account : Entry
    {
        public string Name { set; get; } = string.Empty;

        public string Description { set; get; } = string.Empty;
    }
}
