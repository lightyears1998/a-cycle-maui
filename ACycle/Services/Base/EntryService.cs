using ACycle.Repositories;
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
            CheckIfShadowConversionIsPossible();
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

        private void CheckIfShadowConversionIsPossible()
        {
            _shadowConversionPossible = true;

            foreach (var pi in _modelPropertyInfos)
            {
                if (!pi.CanWrite) { continue; }

                if (!_entityPropertyDictionary.ContainsKey(pi.Name) || _entityPropertyDictionary[pi.Name].PropertyType != pi.PropertyType)
                {
                    _shadowConversionPossible = false;
                    break;
                }
            }
        }

        public virtual TEntity ConvertToEntity(TModel model)
        {
            if (_shadowConversionPossible)
            {
                var entity = new TEntity();
                foreach (var modelPropertyInfo in _modelPropertyInfos)
                {
                    var value = modelPropertyInfo.GetValue(model);
                    var entityPropertyInfo = _entityPropertyDictionary[modelPropertyInfo.Name];
                    if (entityPropertyInfo.CanWrite)
                    {
                        entityPropertyInfo.SetValue(entity, value);
                    }
                }
                return entity;
            }

            throw new NotImplementedException("Shadow conversion is not possible.");
        }

        public virtual IEnumerable<TEntity> ConvertToEntity(IEnumerable<TModel> models)
        {
            return models.Select(ConvertToEntity);
        }

        public virtual TModel ConvertToModel(TEntity entity)
        {
            if (_shadowConversionPossible)
            {
                var model = new TModel();
                foreach (var entityPropertyInfo in _entityPropertyInfos)
                {
                    var value = entityPropertyInfo.GetValue(entity);
                    var modelPropertyInfo = _modelPropertyDictionary[entityPropertyInfo.Name];
                    if (modelPropertyInfo.CanWrite)
                    {
                        modelPropertyInfo.SetValue(model, value);
                    }
                }
                return model;
            }

            throw new NotImplementedException("Shadow conversion is not possible.");
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
                return await UpdateAsync(model);
            }
            else
            {
                return await InsertAsync(model);
            }
        }

        public virtual async Task<TModel> UpdateAsync(TModel model, bool updateTimestamp = true)
        {
            if (updateTimestamp) UpdateTimestamp(model);
            await _entityRepository.UpdateAsync(ConvertToEntity(model));
            return model;
        }

        public virtual async Task<TModel> InsertAsync(TModel model, bool updateTimestamp = true)
        {
            if (updateTimestamp) UpdateTimestamp(model);
            model.CreatedAt = DateTime.Now;
            model.CreatedBy = _configurationService.NodeUuid;
            await _entityRepository.InsertAsync(ConvertToEntity(model));
            return model;
        }

        public virtual async Task<TModel> RemoveAsync(TModel model)
        {
            if (!model.IsRemoved)
            {
                model.RemovedAt = DateTime.Now;
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

    public class EntryServiceEventArgs<TModel> : EventArgs
    {
        public TModel Model;

        public EntryServiceEventArgs(TModel model)
        {
            Model = model;
        }
    }
}
