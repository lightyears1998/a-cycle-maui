namespace ACycle.Entities
{
    public interface ISoftRemovableEntity : IEntity
    {
        public DateTime? RemovedAt { get; set; }
    }
}
