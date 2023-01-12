namespace ACycle.Entities
{
    public interface ISoftRemovableEntity 
    {
        public DateTime? RemovedAt { get; set; }
    }
}
