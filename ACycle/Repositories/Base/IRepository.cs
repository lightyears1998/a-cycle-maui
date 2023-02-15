namespace ACycle.Repositories
{
    public interface IRepository<T>
    {
        event EventHandler<RepositoryEventArgs<T>>? EntityCreated;
        event EventHandler<RepositoryEventArgs<T>>? EntityRemoved;
        event EventHandler<RepositoryEventArgs<T>>? EntityUpdated;
    }
}