using ACycle.Entities;
using ACycle.Models;
using Newtonsoft.Json;

namespace ACycle.EntityRepositories
{
    public class EntryRepository<T>
        where T : IEntryBasedModel, new()
    {
        private readonly EntryRepository _rawRepository;

        public EntryRepository(EntryRepository entryRepository)
        {
            _rawRepository = entryRepository;
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

            await _rawRepository.InsertAsync(entryEntity);
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
            await _rawRepository.UpdateAsync(model.EntryEntity);
            return model;
        }

        public async Task DeleteAsync(T model)
        {
            if (model.EntryEntity == null)
            {
                throw new ArgumentException("Entry is not in database, thus it is not able to delete it from database.");
            }

            await _rawRepository.DeleteAsync(model.EntryEntity);
        }

        public async Task<List<T>> FindAllAsync()
        {
            var entities = await _rawRepository.FindAllAsync(T.EntryContentType);
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
