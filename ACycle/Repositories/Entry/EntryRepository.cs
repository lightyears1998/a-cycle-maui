using ACycle.Entities;

namespace ACycle.Repositories
{
    public class EntryRepository : IEntryRepository
    {
        private readonly List<Type> _entryTypes = new();
        private readonly Dictionary<Type, string> _type2str = new();
        private readonly Dictionary<string, Type> _str2type = new();

        public static EntryRepository Instance => new();

        private EntryRepository()
        {
            RegisterEntries();
        }

        private void RegisterEntries()
        {
            RegisterEntryType<DiaryV1>();
        }

        private void RegisterEntryType<TEntry>()
            where TEntry : Entities.Entry
        {
            var type = typeof(TEntry);
            var contentTypeString = typeof(TEntry).Name;

            if (_entryTypes.Contains(type))
                throw new ArgumentException($"Duplicate type: {type} has been registered.");

            _entryTypes.Add(type);
            _type2str[type] = contentTypeString;
            _str2type[contentTypeString] = type;
        }

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
