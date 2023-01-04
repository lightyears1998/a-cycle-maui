namespace ACycle.Repositories
{
    public class RepositoryEventArgs<T> : EventArgs
    {
        public T Model { get; }

        public RepositoryEventArgs(T model)
        {
            Model = model;
        }
    }
}
