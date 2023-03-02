using System.Numerics;

namespace ACycle.Services
{
    public interface IMetadataService
    {
        Task<string> GetMetadataAsync(string key, string defaultValue);

        Task<bool> GetBoolMetadataAsync(string key, bool? defaultValue);

        Task<TNumber> GetNumberMetadataAsync<TNumber>(string key, TNumber? defaultValue) where TNumber : INumber<TNumber>;

        Task SetMetadataAsync(string key, string value);

        Task SetBoolMetadataAsync(string key, bool value);

        Task SetNumberMetadataAsync<TNumber>(string key, TNumber value) where TNumber : INumber<TNumber>;
    }
}
