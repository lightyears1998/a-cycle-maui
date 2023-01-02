namespace ACycle.Models
{
    public abstract class EntryBasedModelException : Exception
    {
        public EntryBasedModelException(string message) : base(message)
        {
        }
    }

    public class EntryContentTypeUnknownException : EntryBasedModelException
    {
        public string ContentTypeString { get; set; }

        public EntryContentTypeUnknownException(string contentTypeString) : base($"Content type string '{contentTypeString}' is unknown.")
        {
            ContentTypeString = contentTypeString;
        }
    }

    public class ModelTypeUnknownException : EntryBasedModelException
    {
        public Type ModelType { get; set; }

        public ModelTypeUnknownException(Type modelType) : base($"Model type '{modelType.FullName}' is unknown.")
        {
            ModelType = modelType;
        }
    }
}
