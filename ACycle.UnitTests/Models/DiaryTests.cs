using ACycle.Services;
using ACycle.Repositories;
using ACycle.Models;

namespace ACycle.UnitTests.Models
{
    [TestClass]
    public class DiaryTests
    {
        private static readonly string s_test_database_path = "Test.MainDatabase.sqlite3";

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
                Title = "Diary_Save Test Title",
                Content = "Diary_Save test content. The content of a diary for test."
            };
            await s_repo.InsertAsync(diary);
        }

        [TestMethod]
        public async Task Diary_DoubleInsert()
        {
            Diary diary = new()
            {
                Title = "Diary_DoubleInsert Test Title",
            };
            await s_repo.InsertAsync(diary);
            await s_repo.InsertAsync(diary);
        }

        [TestMethod]
        public async Task Diary_Update()
        {
            Diary diary = new()
            {
                Title = "Diary_Update Test Title"
            };
            await s_repo.InsertAsync(diary);

            diary.Title = "Diary_Update Test Title (Updated)";
            await s_repo.UpdateAsync(diary);
        }

        [TestMethod]
        public async Task Diary_SaveAndRead()
        {
            var result = await s_repo.FindAllAsync();
            Assert.IsTrue(result.Count > 0, "Result size should be larger than zero.");
        }

        [TestMethod]
        public async Task Diary_SaveAndDelete()
        {
            int diaryCount = (await s_repo.FindAllAsync()).Count;

            Diary diary = new()
            {
                Title = "Diary_SaveAndDelete Test Title"
            };
            await s_repo.InsertAsync(diary);
            await s_repo.RemoveAsync(diary);

            Assert.AreEqual(diaryCount, (await s_repo.FindAllAsync()).Count);
        }
    }
}
