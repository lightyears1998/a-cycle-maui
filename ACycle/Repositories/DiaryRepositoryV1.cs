using ACycle.Entities;
using ACycle.Services;

namespace ACycle.Repositories
{
    public class DiaryRepositoryV1 : EntryRepository<DiaryV1>
    {
        public DiaryRepositoryV1(IDatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
