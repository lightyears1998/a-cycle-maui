namespace ACycle.Models
{
    public record class Activity : Entry
    {
        public string Name { get; set; } = string.Empty;

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public string Comment { get; set; } = string.Empty;

        public string PreparationStageDescription { get; set; } = string.Empty;

        public string ExecutionStageDescription { get; set; } = string.Empty;

        public string CleanupStageDescription { get; set; } = string.Empty;
    }
}
