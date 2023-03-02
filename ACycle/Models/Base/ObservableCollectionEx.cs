using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ACycle.Models
{
    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        private static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new("Count");
        private static readonly PropertyChangedEventArgs IndexerPropertyChangedEventArgs = new("Item[]");
        private static readonly NotifyCollectionChangedEventArgs CollectionResetEventArgs = new(NotifyCollectionChangedAction.Reset);

        private void OnCountPropertyChanged() => OnPropertyChanged(CountPropertyChangedEventArgs);

        private void OnIndexerPropertyChanged() => OnPropertyChanged(IndexerPropertyChangedEventArgs);

        private void OnCollectionReset() => OnCollectionChanged(CollectionResetEventArgs);

        public ObservableCollectionEx() : base()
        { }

        public ObservableCollectionEx(IEnumerable<T> collection) : base(collection)
        { }

        public ObservableCollectionEx(List<T> list) : base(list)
        { }

        public void NotifyItemChangedAt(int index)
        {
            T item = this[index];
            OnCollectionChanged(new(NotifyCollectionChangedAction.Replace, item, item, index));
        }

        public void InsertSorted(T item, IComparer<T>? comparer = null)
        {
            comparer ??= Comparer<T>.Default;

            int lower = 0;
            int upper = Items.Count;
            while (lower < upper)
            {
                int middle = lower + (upper - lower) / 2;
                int result = comparer.Compare(item, Items[middle]);
                if (result <= 0)
                {
                    upper = middle;
                }
                else
                {
                    lower = middle + 1;
                }
            }

            Insert(lower, item);
        }

        public int BinarySearch(T item, IComparer<T>? comparer = null)
        {
            comparer ??= Comparer<T>.Default;

            int lower = 0;
            int upper = Items.Count;

            while (lower < upper)
            {
                int middle = lower + (upper - lower) / 2;
                int result = comparer.Compare(item, Items[middle]);
                if (result == 0)
                {
                    return middle;
                }
                else if (result < 0)
                {
                    upper = middle;
                }
                else
                {
                    lower = middle + 1;
                }
            }

            return -1;
        }

        public void Reload(IEnumerable<T> items)
        {
            Reload(innerList =>
            {
                foreach (var item in items)
                {
                    innerList.Add(item);
                }
            });
        }

        public void Reload(Action<IList<T>> innerListAction)
        {
            Items.Clear();

            innerListAction(Items);

            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnCollectionReset();
        }

        public async Task ReloadAsync(Func<IList<T>, Task> innerListAction)
        {
            Items.Clear();

            await innerListAction(Items);

            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnCollectionReset();
        }
    }
}
