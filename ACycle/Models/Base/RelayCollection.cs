using System.Windows.Input;

namespace ACycle.Models.Base
{
    public class RelayCollection<T> : ObservableCollectionEx<RelayCollectionItem<T>>
    {
        public Func<RelayCollection<T>, ICommand> EditItemCommandFactory;

        public Func<RelayCollection<T>, ICommand> RemoveItemCommandFactory;

        public RelayCollection(
            Func<RelayCollection<T>, ICommand> editItemCommandFactory,
            Func<RelayCollection<T>, ICommand> removeItemCommandFactory)
        {
            EditItemCommandFactory = editItemCommandFactory;
            RemoveItemCommandFactory = removeItemCommandFactory;
        }

        public RelayCollectionItem<T> WrapItem(T Item)
        {
            return new RelayCollectionItem<T>(Item, EditItemCommandFactory(this), RemoveItemCommandFactory(this));
        }

        public IList<RelayCollectionItem<T>> WrapItems(IList<T> Items)
        {
            return Items.Select(WrapItem).ToList();
        }

        public void Reload(IEnumerable<T> items)
        {
            Reload(innerList =>
            {
                foreach (var item in items)
                {
                    innerList.Add(WrapItem(item));
                }
            });
        }

        public void Reload(Action<IList<T>> innerListAction)
        {
            List<T> plainItems = new();
            innerListAction(plainItems);

            Reload(WrapItems(plainItems));
        }

        public async Task ReloadAsync(Func<IList<T>, Task> innerListAction)
        {
            List<T> plainItems = new();
            await innerListAction(plainItems);
            Reload(WrapItems(plainItems));
        }
    }

    public class RelayCollectionItem<T>
    {
        public T Item;
        public ICommand EditItemCommand;
        public ICommand RemoveItemCommand;

        public RelayCollectionItem(T item, ICommand editItemCommand, ICommand removeItemCommand)
        {
            Item = item;
            EditItemCommand = editItemCommand;
            RemoveItemCommand = removeItemCommand;
        }
    }
}
