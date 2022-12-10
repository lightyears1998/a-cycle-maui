using ACycle.AppServices;
using ACycle.Entities;
using ACycle.Models;
using Newtonsoft.Json;

namespace ACycle.EntityRepositories
{
    public class EntryRepository
    {
        private readonly IDatabaseService _databaseService;

        public EntryRepository(
            IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task InsertAsync(EntryEntity entry)
        {
            await _databaseService.MainDatabase.InsertAsync(entry);
        }

        public async Task UpdateAsync(EntryEntity entry)
        {
            await _databaseService.MainDatabase.UpdateAsync(entry);
        }

        public async Task SaveAsync(EntryEntity entry, bool updateTimestamp = true)
        {
            throw new NotImplementedException();
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
        private readonly IConfigurationService _configurationService;

        public EntryRepository(EntryRepository entryRepository, IConfigurationService configurationService)
        {
            _repository = entryRepository;
            _configurationService = configurationService;
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
                Content = JsonConvert.SerializeObject(model),
                UpdatedBy = _configurationService.NodeUuid,
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
            model.EntryEntity.UpdatedAt = DateTime.UtcNow;
            model.EntryEntity.UpdatedBy = _configurationService.NodeUuid;

            await _repository.UpdateAsync(model.EntryEntity);
            return model;
        }

        public async Task<T> SaveAsync(T model, bool updateStamp = true)
        {
            throw new NotImplementedException();
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
