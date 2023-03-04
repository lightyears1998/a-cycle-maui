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
            s_stringLocalizer ??= App.Current()?.ServiceProvider.GetService<IStringLocalizer<AppStrings>>();
        }

        public string ProvideValue(IServiceProvider serviceProvider)
        {
#if DEBUG
            Guard.IsNotNullOrWhiteSpace(Key);
#endif
            return s_stringLocalizer![Key];
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return (this as IMarkupExtension<string>).ProvideValue(serviceProvider);
        }
    }
}
