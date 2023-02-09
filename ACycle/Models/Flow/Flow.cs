namespace ACycle.Models
{
    public abstract class Flow
    {
        public string Name { set; get; } = "";

        public FlowDirection Direction;
    }

    public enum FlowDirection
    {
        IN,
        OUT
    }
}
