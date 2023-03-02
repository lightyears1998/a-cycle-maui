using ACycle.Entities;
using ACycle.Models;
using AutoMapper;

namespace ACycle.Services
{
    public class SynchronizationEndpointService : Service, ISynchronizationEndpointService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IMapper _mapper;

        public event EventHandler? SynchronizationEndpointChanged;

        public SynchronizationEndpointService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _mapper = GetMapper();
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(config =>
            {
                config.CreateMap<SynchronizationEndpoint, SynchronizationEndpointV1>();
                config.CreateMap<SynchronizationEndpointV1, SynchronizationEndpoint>();
            });
#if DEBUG
            config.AssertConfigurationIsValid();
#endif
            return config.CreateMapper();
        }

        protected SynchronizationEndpointV1 GetEntity(SynchronizationEndpoint model)
        {
            return _mapper.Map<SynchronizationEndpointV1>(model);
        }

        protected SynchronizationEndpoint GetModel(SynchronizationEndpointV1 entity)
        {
            return _mapper.Map<SynchronizationEndpoint>(entity);
        }

        protected IList<SynchronizationEndpoint> GetModel(IList<SynchronizationEndpointV1> list)
        {
            return list.Select(GetModel).ToList();
        }

        public async Task<IList<SynchronizationEndpoint>> LoadAsync()
        {
            var list = await _databaseService.MainDatabase.Table<SynchronizationEndpointV1>().ToListAsync();
            return GetModel(list);
        }

        public async Task SaveAsync(SynchronizationEndpoint config)
        {
            await _databaseService.MainDatabase.InsertOrReplaceAsync(GetEntity(config));
            SynchronizationEndpointChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task RemoveAsync(SynchronizationEndpoint config)
        {
            await _databaseService.MainDatabase.DeleteAsync(GetEntity(config));
            SynchronizationEndpointChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
