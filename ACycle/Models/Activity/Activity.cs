﻿namespace ACycle.Models
{
    public record class Activity : Entry
    {
        public string Name { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string PreparationStageDescription { get; set; } = string.Empty;

        public string ExecutionStageDescription { get; set; } = string.Empty;

        public string CleanupStageDescription { get; set; } = string.Empty;
    }
}
