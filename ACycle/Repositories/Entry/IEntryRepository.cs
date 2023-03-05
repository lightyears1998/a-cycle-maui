namespace ACycle.Repositories
{
    public interface IEntryRepository
    {
        string GetContentTypeStringFromType(Type type);

        Type GetTypeFromContentTypeString(string contentTypeString);
    }
}
