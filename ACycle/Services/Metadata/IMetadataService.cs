namespace ACycle.Services
{
    public interface IMetadataService
    {
        Task<Guid> GetNodeUuidAsync();

        Task<string> GetMetadataAsync(string key, string defaultValue);
    }
}
