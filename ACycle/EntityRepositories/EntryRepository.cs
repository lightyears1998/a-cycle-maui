using ACycle.AppServices;
using ACycle.Entities;
using ACycle.Models;
using Newtonsoft.Json;

namespace ACycle.EntityRepositories
{
    public class EntryRepository<T>
        where T : IEntryBasedModel
    {
        private IDatabaseService _databaseService;

        public EntryRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
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
                UpdatedBy = Guid.NewGuid(),
            };

            await _databaseService.MainDatabase.InsertAsync(entryEntity);
            model.EntryEntity = entryEntity;
            return model;
        }

        public async Task<T> UpdateAsync(T model)
        {
            if (model.EntryEntity == null)
            {
                throw new ArgumentException("Entry is not in database. Use SaveAsync() to save the entity into database first.");
            }

            await _databaseService.MainDatabase.UpdateAsync(model.EntryEntity);
            return model;
        }

        public async Task<T> SaveAsync(T model)
        {
            throw new NotImplementedException();
        }

        public async Task SaveIfNewOrFresherAsync(T model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> FindAllAsync()
        {
            //var query = await _databaseService.MainDatabase.Table<EntryEntity>().Where(v => v.ContentType.Equals(T.EntryContentType)).ToListAsync();
            //return new List<T>();
            throw new NotImplementedException();
        }
    }
}
