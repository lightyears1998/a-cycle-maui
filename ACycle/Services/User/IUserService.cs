using ACycle.Models;

namespace ACycle.Services
{
    public interface IUserService
    {
        Task<UserInfo> GetUserInfoAsync();

        Task SaveUserInfoAsync(UserInfo userInfo);
    }
}
