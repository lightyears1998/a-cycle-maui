using ACycle.Models;
using ACycle.Repositories;
using ACycle.Repositories.Entry;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Web;
using Websocket.Client;

namespace ACycle.Services.Synchronization
{
    /// <summary>
    /// TODO refactoring is needed. Consider rewrite using finite state machine.
    /// </summary>
    public partial class SynchronizationWorker
    {
        private readonly SynchronizationEndpoint _endpoint;
        private readonly ILogger _logger;

        private readonly IConfigurationService _configurationService;
        private readonly IEntryRepository _entryRepository;

        public SynchronizationWorker(
            SynchronizationEndpoint endpoint,
            ILogger logger,
            IConfigurationService configurationService,
            IEntryRepository entryRepository)
        {
            _endpoint = endpoint;
            _logger = logger;

            _configurationService = configurationService;
            _entryRepository = entryRepository;
        }

        public async Task SyncAsync()
        {
            _logger.LogInformation("Sync started for endpoint {Endpoint}.", _endpoint.BriefDescription);

            var userId = await GetUserId().ConfigureAwait(false);
            _logger.LogInformation("Got userId {UserId}.", userId);

            var token = await GetTokenAsync(userId).ConfigureAwait(false);
            _logger.LogInformation("Got token {Token}.", token);

            var metadata = await _entryRepository.GetAllMetadataAsync().ConfigureAwait(false);
            _logger.LogInformation("Metadata of entries are collected. Entry Count: {EntryCount}.", metadata.Count);

            await DoFullSyncAsync(token, metadata).ConfigureAwait(false);
            _logger.LogInformation("Full sync completed.");

            _logger.LogInformation("Sync completed for endpoint {Endpoint}.", _endpoint.BriefDescription);
        }

        private static void ThrowExceptionIfNull<T>([NotNull] T? value, string message)
        {
            if (value == null)
                throw new SynchronizationException(message);
        }

        private Uri GetHttpRequestUri(string pathAndQuery)
        {
            return new Uri(_endpoint.HttpUri, pathAndQuery);
        }

        private static async Task<TPayload> MakeHttpRequest<TPayload>(HttpRequestMessage message)
            where TPayload : class, new()
        {
            using HttpClient client = new();

            var responseMessage = await client.SendAsync(message);
            var response = JsonConvert.DeserializeObject<HttpResponse<TPayload>>(await responseMessage.Content.ReadAsStringAsync());
            ThrowExceptionIfNull(response, responseMessage.ToString());

            if (response.errors.Length > 0)
            {
                throw new SynchronizationException(string.Join("\n", response.errors.Select(err => err.ToString())));
            }

            return response.payload;
        }

        private async Task<string> GetUserId()
        {
            HttpRequestMessage message = new()
            {
                Method = HttpMethod.Get,
                RequestUri = GetHttpRequestUri($"users?username={HttpUtility.UrlEncode(_endpoint.Username)}")
            };

            var payload = await MakeHttpRequest<GetUserIdPayload>(message).ConfigureAwait(false);
            var user = payload.user;
            ThrowExceptionIfNull(user, "User doesn't exist on the server.");

            return user.id;
        }

        private async Task<string> GetTokenAsync(string userId)
        {
            HttpRequestMessage message = new()
            {
                Method = HttpMethod.Post,
                RequestUri = GetHttpRequestUri($"users/{userId}/jwt-tokens"),
                Content = new StringContent(
                    JsonConvert.SerializeObject(
                        new Dictionary<string, string> {
                            {
                                "passwordSha256", _endpoint.PasswordSha256
                            }
                        }),
                    new MediaTypeHeaderValue("application/json"))
            };

            var payload = await MakeHttpRequest<GetTokenPayload>(message).ConfigureAwait(false);
            var token = payload.token;
            ThrowExceptionIfNull(token, "Fail to get token.");

            return token;
        }

        private async Task DoFullSyncAsync(string token, List<EntryMetadata> metadata)
        {
            using WebsocketClient client = new(_endpoint.WsUri, () =>
            {
                var client = new ClientWebSocket();
                client.Options.SetRequestHeader("Authorization", $"Bearer {token}");
                client.Options.SetRequestHeader("A-Cycle-Peer-Node-Uuid", _configurationService.NodeUuid.ToString());
                return client;
            });

            await RunFullSyncProtocolAsync(client, metadata).ConfigureAwait(false);
        }

        private Task RunFullSyncProtocolAsync(WebsocketClient client, List<EntryMetadata> metadata)
        {
            var tcs = new TaskCompletionSource();

            var sendMessage = (WebSocketMessage message) =>
            {
                var plainMessage = JsonConvert.SerializeObject(message)!;
                client.Send(plainMessage);
                _logger.LogInformation("Sent: {Message}.", plainMessage);
            };

            var getMetadataInRange = (int skip) =>
            {
                if (metadata.Count == 0)
                {
                    return Array.Empty<EntryMetadata>();
                }

                skip = Math.Max(skip, 0);
                int index = Math.Min(metadata.Count - 1, skip);
                int count = Math.Min(20, metadata.Count - skip);
                return metadata.GetRange(index, count).ToArray();
            };

            var serverSaidGoodbye = false;
            var clientSaidGoodbye = false;
            var metaQueryCount = 0;
            var metaQueryIndex = 0;
            var entryResponseCount = 0;

            var queryMetadata = () =>
            {
                sendMessage(new WebSocketMessage
                {
                    type = "sync-full-meta-query",
                    payload = new SyncFullMetaQueryPayload
                    {
                        skip = metaQueryIndex
                    }
                });
                metaQueryCount++;
            };

            var shouldSayGoodbye = () =>
            {
                return metaQueryCount <= entryResponseCount;
            };

            var meetExitCondition = () =>
            {
                return serverSaidGoodbye && clientSaidGoodbye;
            };

            client.MessageReceived.Subscribe(async socketMessage =>
            {
                try
                {
                    var plainSocketMessage = socketMessage.ToString();
                    _logger.LogInformation("Received: {Message}.", plainSocketMessage);

                    var message = JsonConvert.DeserializeObject<WebSocketMessage>(plainSocketMessage);
                    Guard.IsNotNull(message);

                    var session = message.session;
                    var payload = message.payload;

                    switch (message.type)
                    {
                        case "handshake":
                            var handshakePayload = (payload as JObject)!.ToObject<HandshakePayload>()!;
                            if (handshakePayload.clientUuid != _configurationService.NodeUuid)
                            {
                                throw new SynchronizationException("WTF? Client Uuid doesn't match! What a terrible failure?!");
                            }
                            break;

                        case "sync-recent-request":
                            sendMessage(new WebSocketMessage
                            {
                                type = "sync-recent-response",
                                errors = new NotImplementedException[] { new NotImplementedException() }
                            });
                            break;

                        case "sync-full-meta-query":
                            var metaQueryPayload = (payload as JObject)!.ToObject<SyncFullMetaQueryPayload>()!;
                            var skip = metaQueryPayload.skip;
                            var partialMetadata = getMetadataInRange(skip);
                            sendMessage(new WebSocketMessage
                            {
                                session = session,
                                type = "sync-full-meta-response",
                                payload = new SyncFullMetaResponsePayload
                                {
                                    skip = skip,
                                    currentCursor = { },
                                    entryMetadata = partialMetadata.ToArray()
                                }
                            });
                            break;

                        case "sync-full-meta-response":
                            var metaResponsePayload = (payload as JObject)!.ToObject<SyncFullMetaResponsePayload>()!;

                            if (metaResponsePayload.entryMetadata.Length == 0)
                            {
                                --metaQueryCount;
                                break;
                            }

                            metaQueryIndex += metaResponsePayload.entryMetadata.Length;
                            queryMetadata();

                            var receivedMetadata = metaResponsePayload.entryMetadata;
                            List<Guid> uuidsToQuery = new List<Guid>();
                            foreach (var meta in receivedMetadata)
                            {
                                var stock = await _entryRepository.FindEntryByUuidAsync(meta.Uuid);
                                if (stock == null || meta.IsFresherThan(stock))
                                {
                                    uuidsToQuery.Add(meta.Uuid);
                                }
                            }
                            sendMessage(new WebSocketMessage
                            {
                                type = "sync-full-entries-query",
                                payload = new SyncFullEntriesQueryPayload
                                {
                                    uuids = uuidsToQuery.ToArray(),
                                }
                            });
                            break;

                        case "sync-full-entries-query":
                            var entriesQueryPayload = (payload as JObject)!.ToObject<SyncFullEntriesQueryPayload>()!;
                            var uuids = entriesQueryPayload.uuids;
                            var entries = await _entryRepository.FindEntriesByUuidsAsync(uuids);
                            var entryContainers = _entryRepository.BoxEntries(entries);
                            sendMessage(new WebSocketMessage
                            {
                                type = "sync-full-entries-response",
                                payload = new SyncFullEntriesResponsePayload
                                {
                                    entries = entryContainers.ToArray(),
                                }
                            });
                            break;

                        case "sync-full-entries-response":
                            var entriesResponsePayload = (payload as JObject)!.ToObject<SyncFullEntriesResponsePayload>()!;
                            var incomingEntries = entriesResponsePayload.entries;
                            foreach (var container in incomingEntries)
                            {
                                var incomingEntry = _entryRepository.UnboxEntryContainer(container);
                                var stockEntry = await _entryRepository.FindEntryByUuidAsync(incomingEntry.Uuid);
                                if (stockEntry == null || incomingEntry.IsFresherThan(stockEntry))
                                {
                                    await _entryRepository.SaveEntry(incomingEntry);
                                }
                            }
                            entryResponseCount++;
                            break;

                        case "goodbye":
                            serverSaidGoodbye = true;
                            break;

                        default:
                            throw new SynchronizationException($"Unrecognized message: {message.type}.");
                    }

                    if (shouldSayGoodbye())
                    {
                        client.SendInstant(JsonConvert.SerializeObject(new WebSocketMessage
                        {
                            session = "goodbye",
                            type = "goodbye"
                        })).Wait();
                        clientSaidGoodbye = true;
                    }

                    if (meetExitCondition())
                    {
                        tcs.TrySetResult();
                    }
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }, ex => tcs.TrySetException(ex));

            client.Start().ContinueWith((_) =>
            {
                queryMetadata();
            });

            return tcs.Task;
        }
    }
}
