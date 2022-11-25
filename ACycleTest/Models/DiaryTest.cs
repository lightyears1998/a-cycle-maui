using ACycle.AppServices.Impl;
using ACycle.EntityRepositories;
using ACycle.Models;

namespace ACycle.UnitTests.Models
{
    [TestClass]
    public class DiaryTest
    {
        private static readonly string s_test_database_path = "TestDatabase.sqlite3";

        private static readonly DatabaseService s_db = new(s_test_database_path);

        private static readonly EntryRepository<Diary> s_repo = new(s_db);

        [ClassInitialize]
        public static async Task Initialize(TestContext _)
        {
            await s_db.Initialize();
        }

        [TestMethod]
        public void Diary_Create()
        {
            _ = new Diary();
        }

        [TestMethod]
        public async Task Diary_Insert()
        {
            Diary diary = new()
            {
                Title = "Test Title",
                Content = "The content of a diary for test."
            };
            await s_repo.InsertAsync(diary);
        }

        [TestMethod]
        public async Task Diary_Read()
        {
            await s_repo.FindAllAsync();
        }
    }
}
