namespace ACycle.Models
{
    public abstract class Transaction : IModel
    {
        public string Comments { set; get; } = string.Empty;
    }
}
