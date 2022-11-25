﻿using ACycle.AppServices;
using ACycle.Entities;
using ACycle.Models;
using Newtonsoft.Json;

namespace ACycle.EntityRepositories
{
    public class EntryRepository<T>
        where T : EntryBasedModel
    {
        private IDatabaseService _databaseService;

        public EntryRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<T> InsertAsync(T model)
        {
            if (model.Entry != null)
            {
                throw new ArgumentException("Entry is already in database.");
            }

            model.Entry = new EntryEntity()
            {
                ContentType = model.EntryContentType,
                Content = JsonConvert.SerializeObject(model),
                UpdatedBy = Guid.NewGuid(),
            };
            await _databaseService.MainDatabase.InsertAsync(model.Entry);
            return model;
        }

        public async Task<T> UpdateAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<T> SaveAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SaveIfNewOrFresherAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> FindAllAsync()
        {
            var query = await _databaseService.MainDatabase.Table<EntryEntity>().Where(v => v.ContentType.Equals(T.EntryContentType)).ToListAsync();
            return new List<T>();
        }
    }
}
