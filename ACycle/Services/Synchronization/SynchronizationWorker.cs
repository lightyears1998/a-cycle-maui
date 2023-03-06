﻿using ACycle.Models;
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

            var userId = await GetUserId();
            _logger.LogInformation("Got userId {UserId}.", userId);

            var token = await GetTokenAsync(userId);
            _logger.LogInformation("Got token {Token}.", token);

            var metadata = await _entryRepository.GetAllMetadataAsync();
            _logger.LogInformation("Metadata of entries are collected. Entry Count: {EntryCount}.", metadata.Count);

            await DoFullSyncAsync(token, metadata);
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

            var payload = await MakeHttpRequest<GetUserIdPayload>(message);
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

            var payload = await MakeHttpRequest<GetTokenPayload>(message);
            var token = payload.token;
            ThrowExceptionIfNull(token, "Fail to get token.");

            return token;
        }

        private Task DoFullSyncAsync(string token, IList<EntryMetadata> metadata)
        {
            var tcs = new TaskCompletionSource();

            using WebsocketClient client = new(_endpoint.WsUri, () =>
            {
                var client = new ClientWebSocket();
                client.Options.SetRequestHeader("Authorization", $"Bearer {token}");
                client.Options.SetRequestHeader("A-Cycle-Peer-Node-Uuid", _configurationService.NodeUuid.ToString());
                return client;
            });

            var sendMessage = (WebSocketMessage message) =>
            {
                client.Send(JsonConvert.SerializeObject(message));
            };

            client.MessageReceived.Subscribe(socketMessage =>
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
                                errors = new Exception[] { new NotImplementedException() }
                            });
                            break;

                        case "sync-full-meta-query":
                            var metaQueryPayload = (payload as JObject)!.ToObject<SyncFullMetaQueryPayload>()!;
                            var skip = metaQueryPayload.skip;
                            break;

                        default:
                            throw new SynchronizationException($"Unrecognized message: {message.type}.");
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }, tcs.SetException);

            _ = client.Start();
            return tcs.Task;
        }
    }
}
