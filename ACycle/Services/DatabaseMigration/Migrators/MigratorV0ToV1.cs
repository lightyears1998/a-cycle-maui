using ACycle.Entities;
using ACycle.Entities.Schemas.V0;
using ACycle.Helpers;
using CommunityToolkit.Diagnostics;
using Newtonsoft.Json;
using SQLite;
using System.Text;

namespace ACycle.Services.DatabaseMigration.Migrators
{
    [MigratorSchemaVersion(Source = 0, Destination = 1)]
    public class MigratorV0ToV1 : Migrator
    {
        private StringBuilder _reportBuilder = new();

        private string _reportSeparator = "\n===============================\n";

        public static async Task<bool> CheckIfV0DatabaseAsync(SQLiteAsyncConnection database)
        {
            var isV0Database = false; // v0 database, aka. Godot version database
            try
            {
                await database.Table<EntryV0>().CountAsync();
                isV0Database = true;
            }
            catch (SQLiteException) { }
            return isV0Database;
        }

        private async Task CreateTablesAsync(SQLiteAsyncConnection connection)
        {
            DatabaseServiceV1 databaseService = new(connection);
            await databaseService.CreateTablesAsync();
        }

        private async Task DropRedundantTablesAsync(SQLiteAsyncConnection connection)
        {
            string[] tableNames = new[] { "entry_history", "peer_node", "metadata" };
            foreach (var tableName in tableNames)
            {
                await connection.ExecuteAsync($"DROP TABLE IF EXISTS {tableName}");
            }
        }

        public override async Task<string> MigrateAsync(SQLiteAsyncConnection connection)
        {
            var isV0Database = await CheckIfV0DatabaseAsync(connection);
            if (!isV0Database)
            {
                ThrowHelper.ThrowInvalidDataException("Database is not a valid database: its schema is unknown.");
            }

            await DropRedundantTablesAsync(connection);
            await CreateTablesAsync(connection);
            await MigrateDiariesAsync(connection);

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

        private async Task MigrateDiariesAsync(SQLiteAsyncConnection connection)
        {
            var oldDiaryEntries = await connection.Table<EntryV0>().Where(entry => entry.ContentType == "diary").ToListAsync();
            _reportBuilder.AppendLine($"Total: {oldDiaryEntries.Count}");

            foreach (var oldDiaryEntry in oldDiaryEntries)
            {
                ParseDiaryV0(oldDiaryEntry, out DiaryV1 newDiary);
                await connection.InsertAsync(newDiary);
            }
        }

        public override async Task PostMigrateAsync(SQLiteAsyncConnection connection)
        {
            string[] tableNames = new[] { "entry" };
            foreach (var tableName in tableNames)
            {
                await connection.ExecuteAsync($"DROP TABLE IF EXISTS {tableName}");
                await connection.ExecuteAsync($"DELETE FROM sqlite_sequence WHERE name = ?", tableName);
            }
        }
    }
}
