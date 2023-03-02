namespace ACycle.Services.Synchronization
{
    public abstract class EntryRegistryException : Exception
    {
        public EntryRegistryException(string message) : base(message)
        {
        }
    }

    public class ContentTypeStringUnknownException : EntryRegistryException
    {
        public string ContentTypeString { get; set; }

        public ContentTypeStringUnknownException(string contentTypeString) : base($"Content Type string '{contentTypeString}' is unknown.")
        {
            ContentTypeString = contentTypeString;
        }
    }

    public class EntryTypeUnknownException : EntryRegistryException
    {
        public Type Type { get; set; }

        public EntryTypeUnknownException(Type type) : base($"Entity Type '{type.FullName}' is unknown.")
        {
            Type = type;
        }
    }
}