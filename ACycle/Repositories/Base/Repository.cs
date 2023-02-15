namespace ACycle.Repositories
{
    public abstract class Repository<T> : IRepository<T>
    {
        public event EventHandler<RepositoryEventArgs<T>>? EntityCreated;

        public event EventHandler<RepositoryEventArgs<T>>? EntityUpdated;

        public event EventHandler<RepositoryEventArgs<T>>? EntityRemoved;

        protected virtual void OnEntityCreated(T entityCreated)
        {
            EntityCreated?.Invoke(this, new(entityCreated));
        }

        protected virtual void OnEntityUpdated(T entityUpdated)
        {
            EntityUpdated?.Invoke(this, new(entityUpdated));
        }

        protected virtual void OnEntityRemoved(T entityRemoved)
        {
            EntityRemoved?.Invoke(this, new(entityRemoved));
        }
    }

    public class RepositoryEventArgs<T> : EventArgs
    {
        public T Entity { get; }

        public RepositoryEventArgs(T entity)
        {
            Entity = entity;
        }
    }
}
