namespace ACycle.Models
{
    public class Activity
    {
        public string Name { get; set; } = string.Empty;

        public List<string> Values { get; set; } = new();
    }
}
