namespace ACycle.Models
{
    public abstract record class Flow
    {
        public string Name { set; get; } = "";

        public FlowDirection Direction { set; get; }
    }

    public enum FlowDirection
    {
        IN,
        OUT
    }
}
