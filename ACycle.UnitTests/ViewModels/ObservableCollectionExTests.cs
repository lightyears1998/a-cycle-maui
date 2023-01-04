using ACycle.ViewModels;

namespace ACycle.UnitTests.ViewModels
{
    [TestClass]
    public class ObservableCollectionExTests
    {
        private static IList<ObservableCollectionEx<double>> MakeCollections()
        {
            return new List<ObservableCollectionEx<double>>{
                new(),
                new(new double[] { 1, 2, 3, 4, 5, 6, 7, 8 })
            };
        }

        private static bool IsSortedList<T>(IList<T> list) where T : IComparable<T>
        {
            var comparer = Comparer<T>.Default;

            for (int i = 1; i < list.Count; i++)
            {
                if (comparer.Compare(list[i - 1], list[i]) > 0)
                {
                    return false;
                }
            }
            return true;
        }

        [TestMethod]
        public void ObservableCollectionEx_BinarySearch()
        {
            var collections = MakeCollections();
            foreach (var collection in collections)
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    var item = collection[i];
                    Assert.AreEqual(i, collection.BinarySearch(item));
                }

                Assert.IsTrue(collection.BinarySearch(99) < 0);
                Assert.IsTrue(collection.BinarySearch(-1) < 0);
            }
        }

        [TestMethod]
        public void ObservableCollectionEx_InsertSorted()
        {
            var collections = MakeCollections();
            foreach (var collection in collections)
            {
                collection.InsertSorted(new Random().NextDouble());
                foreach (var item in new HashSet<double>(collection))
                {
                    collection.InsertSorted(item - 0.5);
                    collection.InsertSorted(item + 0.5);
                    collection.InsertSorted(item);
                    collection.InsertSorted(10 * new Random().NextDouble());
                }
                Assert.IsTrue(IsSortedList(collection));
            }
        }
    }
}
