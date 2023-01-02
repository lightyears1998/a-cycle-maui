using Newtonsoft.Json;

namespace ACycle.UnitTests.Entities
{
    [TestClass]
    public class EntryTests
    {
        [TestMethod]
        public void Entry_Serialize()
        {
            ACycle.Entities.Entry entryEntity = new();
            _ = JsonConvert.SerializeObject(entryEntity);
        }
    }
}
