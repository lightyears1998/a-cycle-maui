﻿using ACycle.Repositories;
using SQLite;
using System.Reflection;

namespace ACycle.Services
{
    public class EntryService<TEntity, TModel> : Service, IEntryService<TEntity, TModel>
        where TEntity : Entities.Entry, new()
        where TModel : Models.Entry, new()
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEntryRepository<TEntity> _entityRepository;

        private readonly PropertyInfo[] _entityPropertyInfos =
            typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        private readonly PropertyInfo[] _modelPropertyInfos =
            typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        private readonly Dictionary<string, PropertyInfo> _entityPropertyDictionary = new();
        private readonly Dictionary<string, PropertyInfo> _modelPropertyDictionary = new();
        private bool _shadowConversionPossible;

        public event EventHandler<EntryServiceEventArgs<TModel>>? ModelCreated;

        public event EventHandler<EntryServiceEventArgs<TModel>>? ModelUpdated;

        public event EventHandler<EntryServiceEventArgs<TModel>>? ModelRemoved;

        protected void OnModelCreated(TModel model)
        {
            ModelCreated?.Invoke(this, new(model));
        }

        protected void OnModelUpdated(TModel model)
        {
            ModelUpdated?.Invoke(this, new(model));
        }

        protected void OnModelRemoved(TModel model)
        {
            ModelRemoved?.Invoke(this, new(model));
        }

        public EntryService(IConfigurationService configurationService, IEntryRepository<TEntity> entityRepository)
        {
            _configurationService = configurationService;
            _entityRepository = entityRepository;
            _entityRepository.EntityCreated += (_, args) => OnModelCreated(ConvertToModel(args.Entity));
            _entityRepository.EntityUpdated += (_, args) => OnModelUpdated(ConvertToModel(args.Entity));
            _entityRepository.EntityRemoved += (_, args) => OnModelRemoved(ConvertToModel(args.Entity));

            InitializePropertyDictionaries();
            CheckIfImplicitConversionIsPossible();
        }

        private void InitializePropertyDictionaries()
        {
            foreach (var pi in _entityPropertyInfos)
            {
                _entityPropertyDictionary[pi.Name] = pi;
            }

            foreach (var pi in _modelPropertyInfos)
            {
                _modelPropertyDictionary[pi.Name] = pi;
            }
        }

        private static bool CheckIfImplicitConversionIsPossible<TSource, TDestination>(
            IList<PropertyInfo> sourcePropertyInfos,
            IDictionary<string, PropertyInfo> destinationPropertyDictionary)
            where TSource : new()
            where TDestination : new()
        {
            foreach (var pi in sourcePropertyInfos)
            {
                if (!pi.CanWrite) continue;

                if (!destinationPropertyDictionary.ContainsKey(pi.Name) || destinationPropertyDictionary[pi.Name].PropertyType != pi.PropertyType)
                {
                    return false;
                }
            }

            return true;
        }

        private void CheckIfImplicitConversionIsPossible()
        {
            _shadowConversionPossible =
                CheckIfImplicitConversionIsPossible<TEntity, TModel>(_entityPropertyInfos, _modelPropertyDictionary)
                &&
                CheckIfImplicitConversionIsPossible<TModel, TEntity>(_modelPropertyInfos, _entityPropertyDictionary);
        }

        protected virtual TDestination ConvertToType<TSource, TDestination>(
            TSource source,
            IList<PropertyInfo> sourcePropertyInfos,
            IDictionary<string, PropertyInfo> destinationPropertyDictionary)
            where TDestination : new()
            where TSource : new()
        {
            if (_shadowConversionPossible)
            {
                var destination = new TDestination();
                foreach (var sourcePropertyInfo in sourcePropertyInfos)
                {
                    var sourceValue = sourcePropertyInfo.GetValue(source);
                    var destPropertyInfo = destinationPropertyDictionary[sourcePropertyInfo.Name];
                    if (destPropertyInfo.CanWrite)
                    {
                        destPropertyInfo.SetValue(destination, sourceValue);
                    }
                }
                return destination;
            }

            throw new NotImplementedException($"Shadow conversion from {typeof(TSource).FullName} to {typeof(TDestination).FullName} is not possible.");
        }

        public virtual TEntity ConvertToEntity(TModel model)
        {
            return ConvertToType<TModel, TEntity>(model, _modelPropertyInfos, _entityPropertyDictionary);
        }

        public virtual IEnumerable<TEntity> ConvertToEntity(IEnumerable<TModel> models)
        {
            return models.Select(ConvertToEntity);
        }

        public virtual TModel ConvertToModel(TEntity entity)
        {
            return ConvertToType<TEntity, TModel>(entity, _entityPropertyInfos, _modelPropertyDictionary);
        }

        public virtual IEnumerable<TModel> ConvertToModel(IEnumerable<TEntity> entities)
        {
            return entities.Select(ConvertToModel);
        }

        protected void UpdateTimestamp(TModel model)
        {
            model.UpdatedAt = DateTime.Now;
            model.UpdatedBy = _configurationService.NodeUuid;
        }

        public virtual async Task<TModel> SaveAsync(TModel model, bool updateTimestamp = true)
        {
            if (updateTimestamp) UpdateTimestamp(model);

            if (model.IsCreated)
            {
                return await UpdateAsync(model, updateTimestamp: false);
            }
            else
            {
                return await InsertAsync(model, updateTimestamp: false);
            }
        }

        public virtual async Task<TModel> UpdateAsync(TModel model, bool updateTimestamp = true)
        {
            if (updateTimestamp)
                UpdateTimestamp(model);

            await _entityRepository.UpdateAsync(ConvertToEntity(model));
            return model;
        }

        public virtual async Task<TModel> InsertAsync(TModel model, bool updateTimestamp = true)
        {
            if (updateTimestamp)
                UpdateTimestamp(model);

            model.CreatedAt = DateTime.Now;
            model.CreatedBy = _configurationService.NodeUuid;
            await _entityRepository.InsertAsync(ConvertToEntity(model));
            return model;
        }

        public virtual async Task UpdateOrInsertAsync(TModel model, bool updateTimestamp = true)
        {
            if (updateTimestamp)
                UpdateTimestamp(model);

            try
            {
                await InsertAsync(model, updateTimestamp: false);
            }
            catch (SQLiteException)
            {
                await UpdateAsync(model, updateTimestamp: false);
            }
        }

        public virtual async Task<TModel> RemoveAsync(TModel model, bool updateTimestamp = true)
        {
            if (!model.IsRemoved)
            {
                model.RemovedAt = DateTime.Now;
                if (updateTimestamp)
                {
                    model.UpdatedAt = DateTime.Now;
                    model.UpdatedBy = _configurationService.NodeUuid;
                }
                await _entityRepository.RemoveAsync(ConvertToEntity(model));
            }

            return model;
        }

        public virtual async Task<List<TModel>> FindAllAsync()
        {
            var entities = await _entityRepository.FindAllAsync();
            return entities.Select(ConvertToModel).ToList();
        }
    }

    public class EntryServiceEventArgs<TEntry> : EventArgs
    {
        public TEntry Entry;

        public EntryServiceEventArgs(TEntry model)
        {
            Entry = model;
        }
    }
}
