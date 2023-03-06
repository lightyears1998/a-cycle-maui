using ACycle.Repositories.Entry;
using Newtonsoft.Json.Linq;

#pragma warning disable CS0649 // Field 'field' is never assigned to, and will always have its default value.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.

namespace ACycle.Services.Synchronization
{
    public partial class SynchronizationWorker
    {
        #region HttpMessage

        private record class HttpResponse<TPayload>
             where TPayload : class, new()
        {
            public JObject[] errors;
            public TPayload payload;
            public DateTime timestamp;
        }

        private record class GetUserIdPayload
        {
            public User? user;

            public record class User
            {
                public string id;
                public string username;
            }
        }

        private record class GetTokenPayload
        {
            public string? token;
        }

        #endregion HttpMessage

        #region WebSocketMessage

        private record class WebSocketMessage
        {
            public string session = Guid.NewGuid().ToString();
            public string type;
            public object[] errors;
            public object payload;
            public DateTime timestamp = DateTime.Now;
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
        private class WebSocketPayloadAttribute : Attribute
        {
            public string MessageType;

            public WebSocketPayloadAttribute(string messageType)
            {
                MessageType = messageType;
            }
        }

        [WebSocketPayload("handshake")]
        private record class HandshakePayload
        {
            public Guid serverUuid;
            public string userId;
            public Guid clientUuid;
        }

        [WebSocketPayload("sync-recent-request")]
        private record class SyncRecentRequestPayload
        {
        }

        [WebSocketPayload("sync-recent-response")]
        private record class SyncRecentResponsePayload
        {
        }

        [WebSocketPayload("sync-full-meta-query")]
        private record class SyncFullMetaQueryPayload
        {
            public int skip = 0;
        }

        [WebSocketPayload("sync-full-meta-response")]
        private record class SyncFullMetaResponsePayload
        {
            public int skip;
            public Dictionary<string, object> currentCursor = new();
            public EntryMetadata[] entryMetadata = Array.Empty<EntryMetadata>();
        }

        [WebSocketPayload("sync-full-entries-query")]
        private record class SyncFullEntriesQueryPayload
        {
            public Guid[] uuids;
        }

        [WebSocketPayload("sync-full-entries-response")]
        private record class SyncFullEntriesResponsePayload
        {
            public EntryContainer[] entries;
        }

        #endregion WebSocketMessage
    }
}
