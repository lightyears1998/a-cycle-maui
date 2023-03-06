namespace ACycle.Entities
{
    public interface IEntryComparable
    {
        Guid Uuid { get; set; }

        DateTime UpdatedAt { get; set; }
    }
}
