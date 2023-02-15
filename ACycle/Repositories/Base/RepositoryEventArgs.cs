namespace ACycle.Repositories
{
    public class RepositoryEventArgs<T> : EventArgs
    {
        public T Entity { get; }

        public RepositoryEventArgs(T entity)
        {
            Entity = entity;
        }
    }
}
