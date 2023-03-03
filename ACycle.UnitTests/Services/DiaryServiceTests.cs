using ACycle.Models;
using ACycle.Repositories;
using ACycle.Services;
using SQLite;

namespace ACycle.UnitTests.Models
{
    [TestClass]
    public class DiaryServiceTests
    {
        private static readonly DatabaseServiceImpl s_db = new("./MainDatabase.sqlite3");

        private static readonly MetadataRepository s_metadataRepository = new(s_db);

        private static readonly MetadataService s_metadataService = new(s_metadataRepository);

        private static readonly ConfigurationService s_config = new(s_metadataService);

        private static readonly DiaryRepository s_repo = new(s_db);

        private static readonly DiaryService s_service = new(s_config, s_repo);

        [ClassInitialize]
        public static async Task Initialize(TestContext _)
        {
            await s_db.InitializeAsync();
            await s_config.InitializeAsync();
            await s_db.CreateTablesAsync();
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
            await s_service.SaveAsync(diary);
        }

        [TestMethod]
        [ExpectedException(typeof(SQLiteException))]
        public async Task Diary_DoubleInsert()
        {
            Diary diary = new()
            {
                Title = "Diary_DoubleInsert Test Title",
            };
            await s_service.InsertAsync(diary);
            await s_service.InsertAsync(diary);
        }

        [TestMethod]
        public async Task Diary_Update()
        {
            Diary diary = new()
            {
                Title = "Diary_Update Test Title"
            };
            await s_service.SaveAsync(diary);

            diary.Title = "Diary_Update Test Title (Updated)";
            await s_service.SaveAsync(diary);
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
            await s_service.SaveAsync(diary);
            await s_service.RemoveAsync(diary);

            Assert.AreEqual(diaryCount, (await s_repo.FindAllAsync()).Count);
        }
    }
}
