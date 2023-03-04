using System.Globalization;

namespace ACycle.Services
{
    public interface IConfigurationService : IService
    {
        Guid NodeUuid { set; get; }

        CultureInfo SystemCultureInfo { get; }
    }
}
