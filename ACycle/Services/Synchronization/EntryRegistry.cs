using ACycle.Entities;

namespace ACycle.Services.Synchronization
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
            RegisterEntry(typeof(ActivityCategoryV1), "activity_category_v1");
            RegisterEntry(typeof(DiaryV1), "diary_v1");
        }

        private void RegisterEntry(Type type, string contentTypeString)
        {
            _type2str[type] = contentTypeString;
            _str2type[contentTypeString] = type;
        }

        public static EntryRegistry Instance => new();

        public Type GetTypeFromContentTypeString(string contentTypeString)
        {
            if (!_str2type.ContainsKey(contentTypeString))
            {
                throw new ContentTypeStringUnknownException(contentTypeString);
            }

            return _str2type[contentTypeString];
        }

        public string GetContentTypeStringFromType(Type type)
        {
            if (!_type2str.ContainsKey(type))
            {
                throw new EntryTypeUnknownException(type);
            }

            return _type2str[type];
        }
    }
}
