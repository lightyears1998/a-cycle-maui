using Newtonsoft.Json;

namespace ACycle.UnitTests.Entities
{
    [TestClass]
    public class EntryTests
    {
        [TestMethod]
        public void Entry_Serialize()
        {
            ACycle.Entities.Entry entry = new();
            _ = JsonConvert.SerializeObject(entry);
        }
    }
}
