namespace ACycle.Repositories
{
    public abstract class EntryRepositoryException
        : Exception
    {
        public EntryRepositoryException(string message) : base(message)
        {
        }
    }

    public class ContentTypeStringUnknownException : EntryRepositoryException
    {
        public string ContentTypeString { get; set; }

        public ContentTypeStringUnknownException(string contentTypeString) : base($"Content Type string '{contentTypeString}' is unknown.")
        {
            ContentTypeString = contentTypeString;
        }
    }

    public class EntryTypeUnknownException : EntryRepositoryException
    {
        public Type Type { get; set; }

        public EntryTypeUnknownException(Type type) : base($"Entity Type '{type.FullName}' is unknown.")
        {
            Type = type;
        }
    }
}
