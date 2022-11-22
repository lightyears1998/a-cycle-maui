using ACycle.DataStructures;

namespace ACycle.UnitTests.DataStructures
{
    [TestClass]
    public class BidirectionalMapTest
    {
        private class Key
        { }

        private class Value
        { }

        [TestMethod]
        public void BidirectionalMap_AddKeyValuePairs()
        {
            BidirectionalMap<Key, Value> map = new();
            Key k1 = new();
            Key k2 = new();
            Key k3 = new();
            Value v1 = new();
            Value v2 = new();
            Value v3 = new();

            map[k1] = v1;
            map[k2] = v2;
            map[k3] = v3;

            Assert.AreEqual(v1, map[k1]);
            Assert.AreEqual(v2, map[k2]);
            Assert.AreEqual(v3, map[k3]);
        }
    }
}
