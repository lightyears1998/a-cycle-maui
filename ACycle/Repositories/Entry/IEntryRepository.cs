using ACycle.Repositories.Entry;

namespace ACycle.Repositories
{
    public interface IEntryRepository
    {
        string GetContentTypeStringFromType(Type type);

        Type GetTypeFromContentTypeString(string contentTypeString);

        Task<List<EntryMetadata>> GetAllMetadataAsync();

        List<EntryContainer> BoxEntries(IEnumerable<Entities.Entry> entries);

        EntryContainer BoxEntry(Entities.Entry entry);

        Entities.Entry UnboxEntryContainer(EntryContainer container);

        List<Entities.Entry> UnboxEntryContainer(IEnumerable<EntryContainer> containers);

        Task<Entities.Entry?> FindEntryByUuidAsync(Guid uuid);

        Task<List<Entities.Entry>> FindEntriesByUuidsAsync(IEnumerable<Guid> uuids);

        Task SaveEntry(Entities.Entry entry);
    }
}
