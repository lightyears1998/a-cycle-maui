using SQLite;

namespace ACycle.Entities.Activity
{
    [Table("entry_activity")]
    public class ActivityV1 : Entry
    {
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("start_date_time")]
        public DateTime? StartDate { get; set; }

        [Column("end_date_time")]
        public DateTime? EndDate { get; set; }

        [Column("preparation_stage_description")]
        public string PreparationStageDescription { get; set; } = string.Empty;

        [Column("execution_stage_description")]
        public string ExecutionStageDescription { get; set; } = string.Empty;

        [Column("cleanup_stage_description")]
        public string CleanupStageDescription { get; set; } = string.Empty;
    }
}
