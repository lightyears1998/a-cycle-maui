namespace ACycle.Repositories
{
    public abstract class Repository<T>
    {
        public delegate void RepositoryEventHandler(object sender, RepositoryEventArgs<T> e);

        public event RepositoryEventHandler? EntityCreated;

        public event RepositoryEventHandler? EntityUpdated;

        public event RepositoryEventHandler? EntityRemoved;

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
}
