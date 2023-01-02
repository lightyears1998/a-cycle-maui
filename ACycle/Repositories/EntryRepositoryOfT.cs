using ACycle.Models;
using Newtonsoft.Json;

namespace ACycle.Repositories
{
    using Entry = Entities.Entry;

    public class EntryRepository<T>
        where T : EntryBasedModel, new()
    {
        private readonly EntryRepository _rawRepository;

        public EntryRepository(EntryRepository entryRepository)
        {
            _rawRepository = entryRepository;
        }

        public async Task<T> InsertAsync(T model)
        {
            if (model.EntryMetadata != null)
            {
                throw new ArgumentException("Entry is already in database. Use SaveAsync() or UpdateAsync() instead of InsertAsync().");
            }

            model.EntryMetadata = new EntryMetadata();
            Entry entry = model.GetEntry();

            await _rawRepository.InsertAsync(entry);
            return model;
        }

        public async Task<T> UpdateAsync(T model)
        {
            if (model.EntryMetadata == null)
            {
                throw new ArgumentException("Entry is not in database. Use SaveAsync() to save the entry into database first.");
            }

            await _rawRepository.UpdateAsync(model.GetEntry());
            return model;
        }

        public async Task RemoveAsync(T model)
        {
            if (model.EntryMetadata == null)
            {
                throw new ArgumentException("Entry is not in database, thus it is not able to delete it from database.");
            }

            await _rawRepository.RemoveAsync(model.GetEntry());
        }

        public async Task<List<T>> FindAllAsync()
        {
            var entryContentType = EntryBasedModelRegistry.Instance.GetEntryContentTypeFromModelType(typeof(T));
            var entries = await _rawRepository.FindAllAsync(entryContentType);

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
