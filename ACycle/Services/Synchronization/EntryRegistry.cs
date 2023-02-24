namespace ACycle.Models
{
    public class EntryRegistry
    {
        private readonly Dictionary<Type, string> _type2str = new();
        private readonly Dictionary<string, Type> _str2type = new();

        private EntryRegistry()
        {
            RegisterEntries();
        }

        private void RegisterEntries()
        {
            RegisterEntry(typeof(Activity), "activity");
            RegisterEntry(typeof(ActivityCategory), "activity_category");
            RegisterEntry(typeof(Diary), "diary");
        }

        private void RegisterEntry(Type modelType, string entryContentType)
        {
            _type2str[modelType] = entryContentType;
            _str2type[entryContentType] = modelType;
        }

        public static EntryRegistry Instance => new();

        public Type GetTypeFromContentTypeString(string contentTypeString)
        {
            if (!_str2type.ContainsKey(contentTypeString))
            {
                throw new EntryContentTypeUnknownException(contentTypeString);
            }

            return _str2type[contentTypeString];
        }

        public string GetContentTypeStringFromType(Type modelType)
        {
            if (!_type2str.ContainsKey(modelType))
            {
                throw new ModelTypeUnknownException(modelType);
            }

            return _type2str[modelType];
        }
    }
}
