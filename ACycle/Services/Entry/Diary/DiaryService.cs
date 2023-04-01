using ACycle.Entities;
using ACycle.Models;
using ACycle.Repositories;

namespace ACycle.Services
{
    public class DiaryService : EntryService<DiaryV1, Diary>, IDiaryService
    {
        public DiaryService(IConfigurationService configurationService, IEntryRepository<DiaryV1> entityRepository) : base(configurationService, entityRepository)
        {
        }
    }

    public interface IDiaryService : IEntryService<DiaryV1, Diary>
    {
    }
}
