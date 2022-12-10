using ACycle.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACycle.UnitTests.Entities
{
    [TestClass]
    public class EntryEntityTests
    {
        [TestMethod]
        public void EntryEntity_Serialize()
        {
            EntryEntity entryEntity = new();
            _ = JsonConvert.SerializeObject(entryEntity);
        }
    }
}
