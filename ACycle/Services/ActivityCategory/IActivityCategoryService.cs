using ACycle.Entities;
using ACycle.Models;

namespace ACycle.Services
{
    public interface IActivityCategoryService : IEntryService<ActivityCategoryV1, ActivityCategory>
    {
        List<ActivityCategory> GetDescendentCategories(ActivityCategory category);

        ActivityCategory? GetParentCategory(ActivityCategory category);
    }
}
