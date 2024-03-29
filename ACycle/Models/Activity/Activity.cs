﻿namespace ACycle.Models
{
    public record class Activity : Entry
    {
        public string Name { get; set; } = string.Empty;

        public DateTime StartDateTime { get; set; } = DateTime.Now;

        public DateTime EndDateTime { get; set; } = DateTime.Now;

        public string Comment { get; set; } = string.Empty;

        public string PreparationStageDescription { get; set; } = string.Empty;

        public string ExecutionStageDescription { get; set; } = string.Empty;

        public string CleanupStageDescription { get; set; } = string.Empty;
    }
}
