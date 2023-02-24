using ACycle.Entities;
using ACycle.Entities.Schemas.V0;
using ACycle.Repositories;
using Newtonsoft.Json;
using SQLite;
using System.Text;

namespace ACycle.Services
{
    public class DatabaseMigrationService : IDatabaseMigrationService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IEntryRepository<DiaryV1> _diaryRepositoryV1;

        public DatabaseMigrationService(IDatabaseService databaseService, IEntryRepository<DiaryV1> diaryRepositoryV1)
        {
            _databaseService = databaseService;
            _diaryRepositoryV1 = diaryRepositoryV1;
        }

        private static DateTime ParseISO8601DateTimeString(string dateTimeString)
        {
            if (!dateTimeString.StartsWith('"'))
                dateTimeString = '"' + dateTimeString;
            if (!dateTimeString.EndsWith('"'))
                dateTimeString += '"';

            return JsonConvert.DeserializeObject<DateTime>(dateTimeString).ToLocalTime();
        }

        public async Task<string> MigrateFromDatabaseVersionGodot(string oldDatabasePath)
        {
            var oldDatabase = new SQLiteAsyncConnection(oldDatabasePath);
            var oldDiaryEntries = await oldDatabase.Table<EntryV0>().Where(entry => entry.ContentType == "diary").ToListAsync();

            const string sectionSeparator = "\n===============================\n";

            StringBuilder builder = new();
            builder.AppendLine($"Total: {oldDiaryEntries.Count}");

            foreach (var entry in oldDiaryEntries)
            {
                dynamic diaryObject = JsonConvert.DeserializeObject(entry.Content)!;

                Guid uuid = entry.Uuid;

                DateTime createdAt = ParseISO8601DateTimeString(entry.CreatedAt);
                DateTime? removedAt = entry.RemovedAt != null && entry.RemovedAt.Length > 0 ? ParseISO8601DateTimeString(entry.RemovedAt) : null;
                DateTime updatedAt = ParseISO8601DateTimeString(entry.UpdatedAt);

                Guid updatedBy = entry.UpdatedBy;

                string content = (string)diaryObject["content"];

                string dateString = (string)diaryObject["date"];
                DateTime date = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(dateString)).LocalDateTime;

                string title = (string)diaryObject["title"];

                builder.AppendLine(sectionSeparator);
                builder.AppendLine($"Uuid: {uuid}\n" +
                    $"CreatedAt: {createdAt} ({entry.CreatedAt})\n" +
                    $"RemovedAt: {removedAt}\n" +
                    $"UpdatedAt: {updatedAt}\nUpdatedBy: {updatedBy}\n" +
                    $"Title: {title}\n" +
                    $"Date: {date} ({dateString})\n" +
                    $"Content:\n{content}");

                {
                    var diary = new DiaryV1()
                    {
                        Uuid = entry.Uuid,
                        CreatedAt = createdAt,
                        CreatedBy = updatedBy,
                        UpdatedAt = updatedAt,
                        UpdatedBy = updatedBy,
                        RemovedAt = removedAt,
                        Title = title,
                        DateTime = date,
                        Content = content,
                    };

                    await _diaryRepositoryV1.InsertAsync(diary);
                }
            }

            _ = oldDatabase.CloseAsync();
            return builder.ToString();
        }
    }
}
