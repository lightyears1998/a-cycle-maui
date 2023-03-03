using CommunityToolkit.Mvvm.ComponentModel;

namespace ACycle.Models.Base
{
    public class RelayCollection<TItem, TRelay> : ObservableCollectionEx<TRelay>
        where TItem : class where TRelay : Relay<TItem>
    {
        protected IList<TRelay> Relays => base.Items;

        public delegate TRelay RelayFactoryMethod(TItem item, RelayCollection<TItem, TRelay> collection);

        public RelayFactoryMethod RelayFactory;

        public RelayCollection(RelayFactoryMethod relayFactory)
        {
            RelayFactory = relayFactory;
        }

        public bool Contains(TItem item)
        {
            return Relays.Any(relay => relay.Item == item);
        }

        public bool Remove(TItem item)
        {
            return Remove(Relays.First(relay => relay.Item == item));
        }

        public TRelay WrapItem(TItem item)
        {
            return RelayFactory(item, this);
        }

        public IList<TRelay> WrapItems(IList<TItem> Items)
        {
            return Items.Select(WrapItem).ToList();
        }

        public void InsertSorted(TItem item, IComparer<TItem>? itemComparer = null)
        {
            if (itemComparer != null)
            {
                RelayComparer<TItem, TRelay> relayComparer = new(itemComparer);
                InsertSorted(WrapItem(item), relayComparer);
            }
            else
            {
                InsertSorted(WrapItem(item));
            }
        }

        public void Reload(IEnumerable<TItem> items)
        {
            Reload(innerList =>
            {
                foreach (var item in items)
                {
                    innerList.Add(WrapItem(item));
                }
            });
        }

        public void Reload(Action<IList<TItem>> innerListAction)
        {
            List<TItem> plainItems = new();
            innerListAction(plainItems);

            Reload(WrapItems(plainItems));
        }

        public async Task ReloadAsync(Func<IList<TItem>, Task> innerListAction)
        {
            List<TItem> plainItems = new();
            await innerListAction(plainItems);
            Reload(WrapItems(plainItems));
        }
    }

    public class Relay<TItem> : ObservableObject
        where TItem : class
    {
        public TItem Item { get; protected set; }

        public Relay(TItem item)
        {
            Item = item;
        }
    }

    public class RelayComparer<TItem, TRelay> : IComparer<TRelay>
        where TItem : class where TRelay : Relay<TItem>
    {
        private IComparer<TItem> _itemComparer;

        public RelayComparer(IComparer<TItem> itemComparer)
        {
            _itemComparer = itemComparer;
        }

        public int Compare(TRelay? x, TRelay? y)
        {
            return _itemComparer.Compare(x?.Item ?? null, y?.Item ?? null);
        }
    }
}
