using System.Globalization;

namespace ACycle.Models
{
    public class UserInfo
    {
        public FeaturePreference FeaturePreference = new();

        public CultureInfo? PreferredLanguage;
    }
}
