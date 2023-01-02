using Newtonsoft.Json;

namespace ACycle.Models
{
    using Entry = Entities.Entry;
    using EntryMetadata = Repositories.EntryMetadata;

    public static class EntryBasedModelExtension
    {
        public static string GetEntryContentType(this EntryBasedModel model)
        {
            return EntryBasedModelRegistry.Instance.GetEntryContentTypeFromModelType(model.GetType());
        }

        public static Entry GetEntry(this EntryBasedModel model)
        {
            model.EntryMetadata ??= new EntryMetadata();

            return new Entry
            {
                Uuid = model.EntryMetadata.Uuid,
                ContentType = model.GetEntryContentType(),
                Content = JsonConvert.SerializeObject(model),
                CreatedAt = model.EntryMetadata.CreatedAt,
                UpdatedAt = model.EntryMetadata.UpdatedAt,
                UpdatedBy = model.EntryMetadata.UpdatedBy,
                RemovedAt = model.EntryMetadata.RemovedAt
            };
        }
    }
}
