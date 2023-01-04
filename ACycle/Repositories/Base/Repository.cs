namespace ACycle.Repositories
{
    public class Repository<T>
    {
        public delegate void RepositoryEventHandler(object sender, RepositoryEventArgs<T> e);

        public event RepositoryEventHandler? ModelCreated;

        public event RepositoryEventHandler? ModelUpdated;

        public event RepositoryEventHandler? ModelRemoved;

        protected virtual void OnModelCreated(T model)
        {
            ModelCreated?.Invoke(this, new(model));
        }

        protected virtual void OnModelUpdated(T model)
        {
            ModelUpdated?.Invoke(this, new(model));
        }

        protected virtual void OnModelRemoved(T model)
        {
            ModelRemoved?.Invoke(this, new(model));
        }
    }
}
