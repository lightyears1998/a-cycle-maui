using ACycle.AppServices;
using ACycle.Entities;
using ACycle.Models;
using Newtonsoft.Json;

namespace ACycle.EntityRepositories
{
    public class EntryRepository
    {
        private readonly IDatabaseService _databaseService;

        private readonly IConfigurationService _configurationService;

        public EntryRepository(
            IDatabaseService databaseService,
            IConfigurationService configurationService)
        {
            _databaseService = databaseService;
            _configurationService = configurationService;
        }

        private void UpdateTimestamp(EntryEntity entry)
        {
            entry.UpdatedAt = DateTime.UtcNow;
            entry.UpdatedBy = _configurationService.NodeUuid;
        }

        public async Task InsertAsync(EntryEntity entry, bool updateTimestamp = true)
        {
            if (updateTimestamp) UpdateTimestamp(entry);

            await _databaseService.MainDatabase.InsertAsync(entry);
        }

        public async Task UpdateAsync(EntryEntity entry, bool updateTimestamp = true)
        {
            if (updateTimestamp) UpdateTimestamp(entry);

            await _databaseService.MainDatabase.UpdateAsync(entry);
        }

        public async Task<List<EntryEntity>> FindAllAsync(string contentType)
        {
            var query = _databaseService.MainDatabase.Table<EntryEntity>().Where(entity => entity.ContentType == contentType);
            return await query.ToListAsync();
        }
    }

    public class EntryRepository<T>
        where T : IEntryBasedModel, new()
    {
        private readonly EntryRepository _repository;

        public EntryRepository(EntryRepository entryRepository)
        {
            _repository = entryRepository;
        }

        public async Task<T> InsertAsync(T model)
        {
            if (model.EntryEntity != null)
            {
                throw new ArgumentException("Entry is already in database. Use SaveAsync() or UpdateAsync() instead of InsertAsync().");
            }

            var entryEntity = new EntryEntity()
            {
                ContentType = T.EntryContentType,
                Content = JsonConvert.SerializeObject(model)
            };

            await _repository.InsertAsync(entryEntity);
            model.EntryEntity = entryEntity;
            return model;
        }

        public async Task<T> UpdateAsync(T model)
        {
            if (model.EntryEntity == null)
            {
                throw new ArgumentException("Entry is not in database. Use SaveAsync() to save the entity into database first.");
            }

            model.EntryEntity.Content = JsonConvert.SerializeObject(model);
            await _repository.UpdateAsync(model.EntryEntity);
            return model;
        }

        public async Task<List<T>> FindAllAsync()
        {
            var entities = await _repository.FindAllAsync(T.EntryContentType);
            var objects = new List<T>();
            foreach (var entity in entities)
            {
                var obj = JsonConvert.DeserializeObject<T>(entity.Content);
                if (obj != null)
                {
                    obj.EntryEntity = entity;
                    objects.Add(obj);
                }
            }
            return objects;
        }
    }
}
