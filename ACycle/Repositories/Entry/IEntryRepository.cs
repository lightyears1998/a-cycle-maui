using ACycle.Repositories.Entry;

namespace ACycle.Repositories
{
    public interface IEntryRepository
    {
        Task<List<EntryMetadata>> GetAllMetadataAsync();

        string GetContentTypeStringFromType(Type type);

        Type GetTypeFromContentTypeString(string contentTypeString);
    }
}
