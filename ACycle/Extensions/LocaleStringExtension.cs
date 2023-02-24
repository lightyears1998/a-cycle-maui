using ACycle.Resources.Strings;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Localization;

namespace ACycle.Extensions
{
    public class LocaleStringExtension : IMarkupExtension<string>
    {
        private static IStringLocalizer<AppStrings>? s_stringLocalizer;

        public string Key { get; set; } = "";

        public LocaleStringExtension()
        {
            TryGetStringLocalizer();
        }

        private void TryGetStringLocalizer()
        {
            try
            {
                s_stringLocalizer ??= App.Current()?.ServiceProvider.GetService<IStringLocalizer<AppStrings>>();
            }
            catch (Exception) { }
        }

        public string ProvideValue(IServiceProvider serviceProvider)
        {
            Guard.IsNotNullOrEmpty(Key);

            string localizedString = Key;
            try
            {
                localizedString = s_stringLocalizer != null ? s_stringLocalizer[Key] : Key;
            }
            catch (Exception) { }

            return localizedString;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return (this as IMarkupExtension<string>).ProvideValue(serviceProvider);
        }
    }
}
