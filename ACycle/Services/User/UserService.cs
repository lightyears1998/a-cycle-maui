using ACycle.Models;
using System.Globalization;

namespace ACycle.Services
{
    public class UserService : Service, IUserService
    {
        private readonly IMetadataService _metadataService;
        private static readonly string KEY_USER_PREFERED_LANGUAGE_CODE = "USER_PREFERED_LANGUAGE_CODE";

        public UserService(IMetadataService metadataService)
        {
            _metadataService = metadataService;
        }

        public async Task<UserInfo> GetUserInfoAsync()
        {
            var prefered_language_code = await _metadataService.GetMetadataAsync(KEY_USER_PREFERED_LANGUAGE_CODE, "");
            var prefered_language = prefered_language_code != "" ? new CultureInfo(prefered_language_code) : null;

            return new UserInfo
            {
                PreferredLanguage = prefered_language
            };
        }

        public async Task SaveUserInfoAsync(UserInfo userInfo)
        {
            await _metadataService.SetMetadataAsync(KEY_USER_PREFERED_LANGUAGE_CODE, userInfo.PreferredLanguage?.Name ?? "");
        }
    }
}
