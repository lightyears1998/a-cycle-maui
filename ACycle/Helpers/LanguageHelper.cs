using System.Globalization;

namespace ACycle.Helpers
{
    public static class LanguageHelper
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
