using ACycle.AppServices.Impl;
using ACycle.EntityRepositories;
using ACycle.Models;

namespace ACycle.UnitTests.Models
{
    [TestClass]
    public class DiaryTest
    {
        [TestMethod]
        public void Diary_Create()
        {
            _ = new Diary();
        }

        [TestMethod]
        public async void Diary_Insert()
        {
            DatabaseService db = new DatabaseService("TestDatabase.sqlite3");
            EntryRepository<Diary> repo = new(db);

            Diary diary = new Diary();
            await db.Initialize();
            await repo.InsertAsync(diary);
        }
    }
}
