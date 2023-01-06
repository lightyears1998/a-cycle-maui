namespace ACycle.Models
{
    public class EntryBasedModelRegistry
    {
        private readonly Dictionary<Type, string> _type2str = new();
        private readonly Dictionary<string, Type> _str2type = new();

        private EntryBasedModelRegistry()
        {
            RegisterEntryBasedModels();
        }

        private void RegisterEntryBasedModels()
        {
            RegisterEntryBasedModel(typeof(Activity), "activity");
            RegisterEntryBasedModel(typeof(ActivityCategory), "activity_category");
            RegisterEntryBasedModel(typeof(Diary), "diary");
        }

        private void RegisterEntryBasedModel(Type modelType, string entryContentType)
        {
            _type2str[modelType] = entryContentType;
            _str2type[entryContentType] = modelType;
        }

        public static EntryBasedModelRegistry Instance => new();

        public Type GetModelTypeFromEntryContentType(string contentTypeString)
        {
            if (!_str2type.ContainsKey(contentTypeString))
            {
                throw new EntryContentTypeUnknownException(contentTypeString);
            }

            return _str2type[contentTypeString];
        }

        public string GetEntryContentTypeFromModelType(Type modelType)
        {
            if (!_type2str.ContainsKey(modelType))
            {
                throw new ModelTypeUnknownException(modelType);
            }

            return _type2str[modelType];
        }
    }
}
