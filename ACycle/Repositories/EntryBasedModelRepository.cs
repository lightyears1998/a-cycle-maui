using ACycle.Models;
using ACycle.Services;
using Newtonsoft.Json;

namespace ACycle.Repositories
{
    using Entry = Entities.Entry;

    public class EntryBasedModelRepository<T> : Repository<T>
        where T : EntryBasedModel, new()
    {
        private readonly IConfigurationService _configurationService;
        private readonly EntryRepository _entryRepository;

        public EntryBasedModelRepository(IConfigurationService configurationService, EntryRepository entryRepository)
        {
            _configurationService = configurationService;
            _entryRepository = entryRepository;
        }

        private void UpdateTimestamp(T model)
        {
            model.EntryMetadata!.UpdatedAt = DateTime.UtcNow;
            model.EntryMetadata!.UpdatedBy = _configurationService.NodeUuid;
        }

        public async Task<T> InsertAsync(T model, bool updateTimestamp = true)
        {
            if (model.EntryMetadata != null)
            {
                throw new ArgumentException("Entry is already in database. Use SaveAsync() or UpdateAsync() instead of InsertAsync().");
            }

            model.EntryMetadata = new EntryMetadata();
            if (updateTimestamp) UpdateTimestamp(model);

            Entry entry = model.GetEntry();
            await _entryRepository.InsertAsync(entry);
            OnModelCreated(model);

            return model;
        }

        public async Task<T> UpdateAsync(T model, bool updateTimestamp = true)
        {
            if (model.EntryMetadata == null)
            {
                throw new ArgumentException("Entry is not in database. Use SaveAsync() to save the entry into database first.");
            }

            if (updateTimestamp) UpdateTimestamp(model);

            await _entryRepository.UpdateAsync(model.GetEntry());
            OnModelUpdated(model);

            return model;
        }

        public async Task<T> SaveAsync(T model, bool updateTimestamp = true)
        {
            if (model.EntryMetadata == null)
            {
                return await InsertAsync(model, updateTimestamp);
            }
            else
            {
                return await UpdateAsync(model, updateTimestamp);
            }
        }

        public async Task RemoveAsync(T model, bool updateTimestamp = true)
        {
            if (model.EntryMetadata == null)
            {
                throw new ArgumentException("Entry is not in database, thus it is not able to delete it from database.");
            }

            model.EntryMetadata.RemovedAt = DateTime.UtcNow;
            if (updateTimestamp) UpdateTimestamp(model);

            await _entryRepository.UpdateAsync(model.GetEntry());
            OnModelRemoved(model);
        }

        public async Task<List<T>> FindAllAsync()
        {
            var entryContentType = EntryBasedModelRegistry.Instance.GetEntryContentTypeFromModelType(typeof(T));
            var entries = await _entryRepository.FindAllAsync(entryContentType);

            var objects = new List<T>();
            foreach (var entry in entries)
            {
                var obj = JsonConvert.DeserializeObject<T>(entry.Content);
                if (obj != null)
                {
                    obj.EntryMetadata = entry.GetMetadata();
                    objects.Add(obj);
                }
            }

            return objects;
        }
    }
}
