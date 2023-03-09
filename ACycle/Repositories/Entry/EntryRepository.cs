using ACycle.Entities;
using ACycle.Repositories.Entry;
using CommunityToolkit.Diagnostics;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace ACycle.Repositories
{
    public class EntryRepository : IEntryRepository
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly List<Type> _entryTypes = new();
        private readonly Dictionary<Type, dynamic> _repos = new();
        private readonly Dictionary<Type, string> _type2str = new();
        private readonly Dictionary<string, Type> _str2type = new();

        public EntryRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RegisterEntries();
        }

        private void RegisterEntries()
        {
            RegisterEntryType<ActivityV1>();
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
            GetRepository(type);
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

        public dynamic GetRepository(Type type)
        {
            Guard.IsNotNull(type);

            if (_repos.TryGetValue(type, out dynamic? value))
            {
                if (value != null)
                    return value;
            }

            if (!_entryTypes.Contains(type))
            {
                ThrowHelper.ThrowArgumentException();
            }

            var repoType = typeof(IEntryRepository<>).MakeGenericType(type);
            dynamic repo = _serviceProvider.GetService(repoType)!;
            Guard.IsNotNull(repo);

            _repos[type] = repo;
            return repo;
        }

        private static async Task<Entities.Entry?> FindEntryInRepository(Guid uuid, dynamic repo)
        {
            return await repo.FindByUuidAsync(uuid).ConfigureAwait(false);
        }

        public async Task<Entities.Entry?> FindEntryByUuidAsync(Guid uuid)
        {
            ConcurrentBag<Entities.Entry> bag = new();

            await Parallel.ForEachAsync(_repos.Values, async (repo, _) =>
            {
                Entities.Entry? entry = await repo.FindByUuidAsync(uuid);
                if (entry != null)
                {
                    bag.Add(entry);
                }
            });

            return bag.FirstOrDefault(defaultValue: null);
        }

        public async Task<List<Entities.Entry>> FindEntriesByUuidsAsync(IEnumerable<Guid> uuids)
        {
            ConcurrentBag<Entities.Entry> bag = new();

            await Parallel.ForEachAsync(uuids, async (uuid, _) =>
            {
                Entities.Entry? entry = await FindEntryByUuidAsync(uuid);
                if (entry != null)
                {
                    bag.Add(entry);
                }
            });

            return bag.ToList();
        }

        public async Task<List<EntryMetadata>> GetAllMetadataAsync()
        {
            List<EntryMetadata> allMetadata = new();
            foreach (var type in _entryTypes)
            {
                dynamic repo = GetRepository(type);
                var metadata = (List<EntryMetadata>)await repo.GetAllMetadataAsync().ConfigureAwait(false);
                allMetadata.AddRange(metadata);
            }
            return allMetadata;
        }

        public EntryContainer BoxEntry(Entities.Entry entry)
        {
            string contentType = GetContentTypeStringFromType(entry.GetType());
            string content = JsonConvert.SerializeObject(entry);
            return new EntryContainer
            {
                Uuid = entry.Uuid,
                CreatedAt = entry.CreatedAt,
                CreatedBy = entry.CreatedBy,
                UpdatedAt = entry.UpdatedAt,
                UpdatedBy = entry.UpdatedBy,
                RemovedAt = entry.RemovedAt,
                ContentType = contentType,
                Content = content,
            };
        }

        public Entities.Entry UnboxEntryContainer(EntryContainer container)
        {
            Type type = GetTypeFromContentTypeString(container.ContentType);
            var entry = (Entities.Entry)JsonConvert.DeserializeObject(container.Content, type)!;
            Guard.IsNotNull(entry);

            return entry;
        }

        public List<EntryContainer> BoxEntries(IEnumerable<Entities.Entry> entries)
        {
            return entries.AsParallel().Select(BoxEntry).ToList();
        }

        public List<Entities.Entry> UnboxEntryContainer(IEnumerable<EntryContainer> containers)
        {
            return containers.AsParallel().Select(UnboxEntryContainer).ToList();
        }

        public async Task SaveEntryAsync(Entities.Entry entry)
        {
            dynamic repo = GetRepository(entry.GetType());
            dynamic runtime_entry = entry;
            await repo.SaveAsync(runtime_entry);
        }
    }
}
