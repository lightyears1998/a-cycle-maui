using ACycle.Entities;
using ACycle.Models;
using ACycle.Repositories;

namespace ACycle.Services
{
    public class ActivityService : EntryService<ActivityV1, Activity>, IActivityService
    {
        public ActivityService(IConfigurationService configurationService, IEntryRepository<ActivityV1> entityRepository) : base(configurationService, entityRepository)
        {
        }
    }

    public interface IActivityService : IEntryService<ActivityV1, Activity>
    {
    }
}
