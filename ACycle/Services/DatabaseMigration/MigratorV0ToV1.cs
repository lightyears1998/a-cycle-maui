using ACycle.Entities;
using ACycle.Entities.Schemas.V0;
using ACycle.Extensions;
using ACycle.Helpers;
using Newtonsoft.Json;
using SQLite;
using System.Text;

namespace ACycle.Services.DatabaseMigration
{
    public class MigratorV0ToV1 : IMigrator
    {
        public long SourceSchemaVersion => 0;

        public long DestinationSchemaVersion => 1;

        private StringBuilder _reportBuilder = new();

        private string _reportSeparator = "\n===============================\n";

        public static async Task<bool> CheckIfGodotVersionDatabaseAsync(SQLiteAsyncConnection database)
        {
            var isGodotVersionDatabase = false;
            try
            {
                await database.Table<EntryV0>().CountAsync();
                isGodotVersionDatabase = true;
            }
            catch (SQLiteException) { }
            return isGodotVersionDatabase;
        }

        private async Task CreateTablesAsync(SQLiteAsyncConnection destinationDatabase)
        {
            DatabaseServiceV1 databaseService = new(destinationDatabase);
            await databaseService.CreateTablesAsync();
        }

        private async Task DropRedundantTablesAsync(SQLiteAsyncConnection destinationDatabase)
        {
            string[] tableNames = new[] { "entry_history", "peer_node", "metadata" };
            foreach (var tableName in tableNames)
            {
                await destinationDatabase.ExecuteAsync($"DROP TABLE IF EXISTS {tableName}");
            }
        }

        public async Task<string> MigrateAsync(SQLiteAsyncConnection sourceDatabase, SQLiteAsyncConnection destinationDatabase)
        {
            await DropRedundantTablesAsync(destinationDatabase);
            await CreateTablesAsync(destinationDatabase);
            await MigrateDiariesAsync(sourceDatabase, destinationDatabase);
            await PostMigrateCleanUp(destinationDatabase);
            await destinationDatabase.SetSchemaAsync(DestinationSchemaVersion);

            return _reportBuilder.ToString();
        }

        private async Task MigrateDiariesAsync(SQLiteAsyncConnection sourceDatabase, SQLiteAsyncConnection destinationDatabase)
        {
            var oldDiaries = await sourceDatabase.Table<EntryV0>().Where(entry => entry.ContentType == "diary").ToListAsync();
            _reportBuilder.AppendLine($"Total: {oldDiaries.Count}");

            foreach (var oldDiary in oldDiaries)
            {
                dynamic oldDiaryObject = JsonConvert.DeserializeObject(oldDiary.Content)!;

                Guid uuid = oldDiary.Uuid;

                DateTime createdAt = DateTimeHelper.ParseISO8601DateTimeString(oldDiary.CreatedAt);
                DateTime? removedAt = oldDiary.RemovedAt != null && oldDiary.RemovedAt.Length > 0 ? DateTimeHelper.ParseISO8601DateTimeString(oldDiary.RemovedAt) : null;
                DateTime updatedAt = DateTimeHelper.ParseISO8601DateTimeString(oldDiary.UpdatedAt);

                Guid updatedBy = oldDiary.UpdatedBy;

                string content = (string)oldDiaryObject["content"];

                string dateString = (string)oldDiaryObject["date"];
                DateTime date = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(dateString)).LocalDateTime;

                string title = (string)oldDiaryObject["title"];

                _reportBuilder.AppendLine(_reportSeparator);
                _reportBuilder.AppendLine($"Uuid: {uuid}\n" +
                    $"CreatedAt: {createdAt} ({oldDiary.CreatedAt})\n" +
                    $"RemovedAt: {removedAt}\n" +
                    $"UpdatedAt: {updatedAt}\nUpdatedBy: {updatedBy}\n" +
                    $"Title: {title}\n" +
                    $"Date: {date} ({dateString})\n" +
                    $"Content:\n{content}");

                var newDiary = new DiaryV1()
                {
                    Uuid = oldDiary.Uuid,
                    CreatedAt = createdAt,
                    CreatedBy = updatedBy,
                    UpdatedAt = updatedAt,
                    UpdatedBy = updatedBy,
                    RemovedAt = removedAt,
                    Title = title,
                    DateTime = date,
                    Content = content,
                };

                await destinationDatabase.InsertAsync(newDiary);
            }
        }

        private async Task PostMigrateCleanUp(SQLiteAsyncConnection destinationDatabase)
        {
            string[] tableNames = new[] { "entry" };
            foreach (var tableName in tableNames)
            {
                await destinationDatabase.ExecuteAsync($"DROP TABLE IF EXISTS {tableName}");
                await destinationDatabase.ExecuteAsync($"DELETE FROM sqlite_sequence WHERE name = ?", tableName);
            }
        }
    }
}
