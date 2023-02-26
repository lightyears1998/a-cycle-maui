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

        private void ParseDiaryV0(EntryV0 oldDiaryEntry, out DiaryV1 newDiary)
        {
            newDiary = new DiaryV1();
            dynamic oldDiaryObject = JsonConvert.DeserializeObject(oldDiaryEntry.Content)!;

            newDiary.Uuid = oldDiaryEntry.Uuid;
            newDiary.CreatedAt = DateTimeHelper.ParseISO8601DateTimeString(oldDiaryEntry.CreatedAt);
            newDiary.CreatedBy = oldDiaryEntry.UpdatedBy;
            newDiary.UpdatedAt = DateTimeHelper.ParseISO8601DateTimeString(oldDiaryEntry.UpdatedAt);
            newDiary.UpdatedBy = oldDiaryEntry.UpdatedBy;
            newDiary.RemovedAt = oldDiaryEntry.RemovedAt != null && oldDiaryEntry.RemovedAt.Length > 0 ? DateTimeHelper.ParseISO8601DateTimeString(oldDiaryEntry.RemovedAt) : null;
            newDiary.Title = (string)oldDiaryObject["title"];
            var dateTimeString = (string)oldDiaryObject["date"];
            newDiary.DateTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(dateTimeString)).LocalDateTime;
            newDiary.Content = (string)oldDiaryObject["content"];

            _reportBuilder.AppendLine(_reportSeparator);
            _reportBuilder.AppendLine($"Uuid: {newDiary.Uuid}\n" +
                $"CreatedAt: {newDiary.CreatedAt} ({oldDiaryEntry.CreatedAt})\n" +
                $"RemovedAt: {newDiary.RemovedAt}\n" +
                $"UpdatedAt: {newDiary.UpdatedAt}\n" +
                $"UpdatedBy: {newDiary.UpdatedBy}\n" +
                $"Title: {newDiary.Title}\n" +
                $"Date: {newDiary.DateTime} ({dateTimeString})\n" +
                $"Content:\n{newDiary.Content}");
        }

        private async Task MigrateDiariesAsync(SQLiteAsyncConnection sourceDatabase, SQLiteAsyncConnection destinationDatabase)
        {
            var oldDiaryEntries = await sourceDatabase.Table<EntryV0>().Where(entry => entry.ContentType == "diary").ToListAsync();
            _reportBuilder.AppendLine($"Total: {oldDiaryEntries.Count}");

            foreach (var oldDiaryEntry in oldDiaryEntries)
            {
                ParseDiaryV0(oldDiaryEntry, out DiaryV1 newDiary);
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
