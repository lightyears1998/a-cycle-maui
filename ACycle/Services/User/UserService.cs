using ACycle.Models;
using System.Globalization;

namespace ACycle.Services
{
    public class UserService : Service, IUserService
    {
        private readonly IMetadataService _metadataService;

        public UserService(IMetadataService metadataService)
        {
            _metadataService = metadataService;
        }

        public async Task<UserInfo> GetUserInfoAsync()
        {
            var preferred_language_code = await _metadataService.GetMetadataAsync(MetadataKeys.PREFERRED_LANGUAGE_CODE, "");
            var preferred_language = preferred_language_code != "" ? new CultureInfo(preferred_language_code) : null;

            return new UserInfo
            {
                PreferredLanguage = preferred_language
            };
        }

        public async Task SaveUserInfoAsync(UserInfo userInfo)
        {
            await _metadataService.SetMetadataAsync(MetadataKeys.PREFERRED_LANGUAGE_CODE, userInfo.PreferredLanguage?.Name ?? "");
        }

        protected static class MetadataKeys
        {
            public const string PREFERRED_LANGUAGE_CODE = "USER_PREFERRED_LANGUAGE_CODE";
        }
    }
}
