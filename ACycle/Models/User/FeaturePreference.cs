namespace ACycle.Models
{
    public record class FeaturePreference
    {
        public bool ActivityEnabled { set; get; } = true;

        public bool DiaryEnabled { set; get; } = true;
    }
}
