using System.Globalization;

namespace ACycle.Models
{
    public class UserInfo
    {
        public FeaturePreference FeaturePreference { set; get; } = new();

        public CultureInfo? PreferredLanguage { set; get; }
    }
}
