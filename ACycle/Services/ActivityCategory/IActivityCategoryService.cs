using ACycle.Models;

namespace ACycle.Services
{
    public interface IActivityCategoryService : IService
    {
        List<ActivityCategory> GetDescendentCategories(ActivityCategory category);

        ActivityCategory? GetParentCategory(ActivityCategory category);

        Task SaveCategory(ActivityCategory category);
    }
}
