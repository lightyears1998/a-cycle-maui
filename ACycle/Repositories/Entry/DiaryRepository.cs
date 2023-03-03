using ACycle.Entities;
using ACycle.Services.Database;

namespace ACycle.Repositories
{
    public class DiaryRepository : EntryRepository<DiaryV1>
    {
        public DiaryRepository(IDatabaseConnectionWrapper connectionWrapper) : base(connectionWrapper)
        {
        }
    }
}
