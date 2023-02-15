namespace ACycle.Models
{
    public abstract class EntryException : Exception
    {
        public EntryException(string message) : base(message)
        {
        }
    }

    public class EntryContentTypeUnknownException : EntryException
    {
        public string ContentTypeString { get; set; }

        public EntryContentTypeUnknownException(string contentTypeString) : base($"Content type string '{contentTypeString}' is unknown.")
        {
            ContentTypeString = contentTypeString;
        }
    }

    public class ModelTypeUnknownException : EntryException
    {
        public Type ModelType { get; set; }

        public ModelTypeUnknownException(Type modelType) : base($"Entity type '{modelType.FullName}' is unknown.")
        {
            ModelType = modelType;
        }
    }
}
