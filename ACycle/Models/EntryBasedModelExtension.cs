using Newtonsoft.Json;

namespace ACycle.Models
{
    using Entry = Entities.Entry;

    public static class EntryBasedModelExtension
    {
        public static string GetEntryContentType(this EntryBasedModel model)
        {
            return EntryBasedModelRegistry.Instance.GetEntryContentTypeFromModelType(model.GetType());
        }

        public static Entry GetEntry(this EntryBasedModel model)
        {
            return new Entry
            {
                Uuid = model.Uuid,
                ContentType = model.GetEntryContentType(),
                Content = JsonConvert.SerializeObject(model),
                CreatedAt = (DateTime)model.EntryMetadata.CreatedAt!,
                CreatedBy = (Guid)model.EntryMetadata.CreatedBy!,
                UpdatedAt = model.EntryMetadata.UpdatedAt,
                UpdatedBy = model.EntryMetadata.UpdatedBy,
                RemovedAt = model.EntryMetadata.RemovedAt
            };
        }
    }
}
