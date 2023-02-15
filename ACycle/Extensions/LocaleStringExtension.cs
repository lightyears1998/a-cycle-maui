using ACycle.Resources.Strings;
using Microsoft.Extensions.Localization;

namespace ACycle.Extensions
{
    public class LocaleStringExtension : IMarkupExtension<string>
    {
        private readonly IStringLocalizer<AppStrings> _stringLocalizer;

        public string Key { get; set; } = "";

        public LocaleStringExtension()
        {
            _stringLocalizer = App.Current()!.ServiceProvider.GetService<IStringLocalizer<AppStrings>>()!;
        }

        public string ProvideValue(IServiceProvider serviceProvider)
        {
            return _stringLocalizer[Key];
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return (this as IMarkupExtension<string>).ProvideValue(serviceProvider);
        }
    }
}
