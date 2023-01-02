using ACycle.Services;
using ACycle.Repositories;
using ACycle.Models;

namespace ACycle.UnitTests.Models
{
    [TestClass]
    public class DiaryTests
    {
        private static readonly string s_test_database_path = "TestDatabase.sqlite3";

        private static readonly DatabaseService s_db = new(s_test_database_path);

        private static readonly ConfigurationService s_config = new(new MetadataRepository(s_db));

        private static readonly EntryRepository<Diary> s_repo = new(new EntryRepository(s_db, s_config));

        [ClassInitialize]
        public static async Task Initialize(TestContext _)
        {
            await s_db.InitializeAsync();
            await s_config.InitializeAsync();
        }

        [TestMethod]
        public void Diary_Create()
        {
            _ = new Diary();
        }

        [TestMethod]
        public async Task Diary_Save()
        {
            Diary diary = new()
            {
                Title = "Test Title",
                Content = "The content of a diary for test."
            };
            await s_repo.InsertAsync(diary);
        }

        [TestMethod]
        public async Task Diary_Update()
        {
            Diary diary = new()
            {
                Title = "Title to be Updated"
            };
            await s_repo.InsertAsync(diary);

            diary.Title = "Updated Title";
            await s_repo.UpdateAsync(diary);
        }

        [TestMethod]
        public async Task Diary_SaveAndRead()
        {
            var result = await s_repo.FindAllAsync();
            Assert.IsTrue(result.Count > 0, "Result size should be larger than zero.");
        }
    }
}
