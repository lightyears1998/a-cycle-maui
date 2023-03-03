using ACycle.Entities;
using ACycle.Models;
using ACycle.Repositories;

namespace ACycle.Services
{
    public class DiaryService : EntryService<DiaryV1, Diary>
    {
        public DiaryService(IConfigurationService configurationService, IEntryRepository<DiaryV1> entityRepository) : base(configurationService, entityRepository)
        {
        }
    }
}
