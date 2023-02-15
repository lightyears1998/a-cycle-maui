using System.Globalization;

namespace ACycle.Helpers
{
    public class LanguageHelper
    {
        public static void SwitchLanguage(CultureInfo culture)
        {
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Thread.CurrentThread.CurrentUICulture = culture;
            });
        }
    }
}
