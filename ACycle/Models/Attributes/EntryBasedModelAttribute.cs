using ACycle.Entities;

namespace ACycle.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EntryBasedModelAttribute : Attribute
    {
        public string EntryContentType { get; } = string.Empty;

        public EntryEntity? Entry = null;

        public EntryBasedModelAttribute(string entryContentType)
        {
            EntryContentType = entryContentType;
        }
    }
}
