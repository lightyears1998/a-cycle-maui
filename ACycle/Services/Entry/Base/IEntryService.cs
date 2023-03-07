namespace ACycle.Services
{
    public interface IEntryService<TEntity, TModel> : IService
        where TEntity : Entities.Entry, new()
        where TModel : Models.Entry, new()
    {
        public event EventHandler<EntryServiceEventArgs<TModel>>? ModelCreated;

        public event EventHandler<EntryServiceEventArgs<TModel>>? ModelUpdated;

        public event EventHandler<EntryServiceEventArgs<TModel>>? ModelRemoved;

        TEntity ConvertToEntity(TModel model);

        IEnumerable<TEntity> ConvertToEntity(IEnumerable<TModel> models);

        TModel ConvertToModel(TEntity entry);

        IEnumerable<TModel> ConvertToModel(IEnumerable<TEntity> entities);

        Task<TModel> SaveAsync(TModel model, bool updateTimestamp = true);

        Task<TModel> RemoveAsync(TModel model, bool updateTimestamp = true);

        Task<List<TModel>> FindAllAsync();
    }
}
