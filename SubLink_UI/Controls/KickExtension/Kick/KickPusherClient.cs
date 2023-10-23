using FlowGraph;
using FlowGraph.Plugin;
using FlowGraph.Process;
using PusherClient;
using System.Text.Json;
using tech.sublink.KickExtension.Kick.Types;
using tech.sublink.KickExtension.Event;
using FlowGraph.Logger;

namespace tech.sublink.KickExtension.Kick;

public class KickPusherClient : IPlugin {
    private Pusher _pusher;
    private readonly string _key = "";
    private readonly string _cluster = "";
    private readonly string _chatroomId = "";

    public KickPusherClient() {
        _pusher = new Pusher(_key, new() {
            Cluster = _cluster,
            Encrypted = true,
            ClientTimeout = TimeSpan.FromSeconds(120)
        });
    }

    public async Task LoadAsync() {
        _pusher.Connected += Pusher_Connected;
        _pusher.ConnectionStateChanged += Pusher_ConnectionStateChanged;
        _pusher.Disconnected += Pusher_Disconnected;
        _pusher.Error += Pusher_Error;
        _pusher.Subscribed += Pusher_Subscribed;

        try {
            await _pusher.ConnectAsync();

            // Bind to events
            _pusher.Bind(@"App\Events\ChatMessageEvent", OnChatMessageEvent);
            _pusher.Bind(@"App\Events\GiftedSubscriptionsEvent", OnGiftedSubscriptionsEvent);
            _pusher.Bind(@"App\Events\SubscriptionEvent", OnSubscriptionEvent);
            _pusher.Bind(@"App\Events\StreamHostEvent", OnStreamHostEvent);
            _pusher.Bind(@"App\Events\UserBannedEvent", OnUserBannedEvent);
            _pusher.Bind(@"App\Events\UserUnbannedEvent", OnUserUnbannedEvent);
            _pusher.Bind(@"App\Events\MessageDeletedEvent", OnMessageDeletedEvent);
            _pusher.Bind(@"App\Events\ChatroomClearEvent", OnChatroomClearEvent);
            _pusher.Bind(@"App\Events\ChatroomUpdatedEvent", OnChatroomUpdatedEvent);
            _pusher.Bind(@"App\Events\PollUpdateEvent", OnPollUpdateEvent);
            _pusher.Bind(@"App\Events\PollDeleteEvent", OnPollDeleteEvent);
            _pusher.Bind(@"App\Events\PinnedMessageCreatedEvent", OnPinnedMessageCreatedEvent);
            _pusher.Bind(@"App\Events\PinnedMessageDeletedEvent", OnPinnedMessageDeletedEvent);

            await _pusher.SubscribeAsync($"chatrooms.{_chatroomId}.v2");
        } catch (Exception) {
            return;
        }

        return;
    }

    public async Task UnloadAsync() {
        _pusher.UnbindAll();
        await _pusher.UnsubscribeAllAsync();
        await _pusher.DisconnectAsync();
    }

    private void Pusher_Connected(object sender) =>
        LogManager.Instance.WriteLine(LogVerbosity.Info, "Kick Pusher client connected");

    private void Pusher_ConnectionStateChanged(object sender, ConnectionState state) =>
        LogManager.Instance.WriteLine(LogVerbosity.Debug, $"Kick Pusher client connection state changed : {state}");

    private void Pusher_Disconnected(object sender) =>
        LogManager.Instance.WriteLine(LogVerbosity.Info, "Kick Pusher client disconnected");

    private void Pusher_Error(object sender, PusherException error) =>
        LogManager.Instance.WriteException(error);

    private void Pusher_Subscribed(object sender, Channel channel) =>
        LogManager.Instance.WriteLine(LogVerbosity.Debug, $"Kick Pusher client subscribed to `{channel.Name}`");

    private void OnChatMessageEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<ChatMessage>(eventData.Data);

        if (data != null)
            foreach (var seq in GraphDataManager.Instance.GraphList) {
                ProcessLauncher.Instance.LaunchSequence(seq, typeof(EventChatMessageNode), 0, data);
            }
    }

    private void OnGiftedSubscriptionsEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<GiftedSubscriptions>(eventData.Data);

        if (data != null)
            foreach (var seq in GraphDataManager.Instance.GraphList) {
                ProcessLauncher.Instance.LaunchSequence(seq, typeof(EventGiftedSubscriptionsNode), 0, data);
            }
    }

    private void OnSubscriptionEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<Subscription>(eventData.Data);

        if (data != null)
            foreach (var seq in GraphDataManager.Instance.GraphList) {
                ProcessLauncher.Instance.LaunchSequence(seq, typeof(EventSubscriptionNode), 0, data);
            }
    }

    private void OnStreamHostEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<StreamHost>(eventData.Data);

        if (data != null)
            foreach (var seq in GraphDataManager.Instance.GraphList) {
                ProcessLauncher.Instance.LaunchSequence(seq, typeof(EventStreamHostNode), 0, data);
            }
    }

    private void OnUserBannedEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<UserBanned>(eventData.Data);

        if (data != null)
            foreach (var seq in GraphDataManager.Instance.GraphList) {
                ProcessLauncher.Instance.LaunchSequence(seq, typeof(EventUserBannedNode), 0, data);
            }
    }

    private void OnUserUnbannedEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<UserUnbanned>(eventData.Data);

        if (data != null)
            foreach (var seq in GraphDataManager.Instance.GraphList) {
                ProcessLauncher.Instance.LaunchSequence(seq, typeof(EventUserUnbannedNode), 0, data);
            }
    }

    private void OnMessageDeletedEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<MessageDeleted>(eventData.Data);

        if (data != null)
            foreach (var seq in GraphDataManager.Instance.GraphList) {
                ProcessLauncher.Instance.LaunchSequence(seq, typeof(EventMessageDeletedNode), 0, data);
            }
    }

    private void OnChatroomClearEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<ChatroomClear>(eventData.Data);

        if (data != null)
            foreach (var seq in GraphDataManager.Instance.GraphList) {
                ProcessLauncher.Instance.LaunchSequence(seq, typeof(EventChatroomClearNode), 0, data);
            }
    }

    private void OnChatroomUpdatedEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<ChatroomUpdated>(eventData.Data);

        if (data != null)
            foreach (var seq in GraphDataManager.Instance.GraphList) {
                ProcessLauncher.Instance.LaunchSequence(seq, typeof(EventChatroomUpdatedNode), 0, data);
            }
    }

    private void OnPollUpdateEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<PollUpdate>(eventData.Data);

        if (data != null)
            foreach (var seq in GraphDataManager.Instance.GraphList) {
                ProcessLauncher.Instance.LaunchSequence(seq, typeof(EventPollUpdateNode), 0, data);
            }
    }

    private void OnPollDeleteEvent(PusherEvent eventData) {
        foreach (var seq in GraphDataManager.Instance.GraphList) {
            ProcessLauncher.Instance.LaunchSequence(seq, typeof(EventPollDeleteNode), 0, null);
        }
    }

    private void OnPinnedMessageCreatedEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<PinnedMessageCreated>(eventData.Data);

        if (data != null)
            foreach (var seq in GraphDataManager.Instance.GraphList) {
                ProcessLauncher.Instance.LaunchSequence(seq, typeof(EventPinnedMessageCreatedNode), 0, data);
            }
    }

    private void OnPinnedMessageDeletedEvent(PusherEvent eventData) {
        foreach (var seq in GraphDataManager.Instance.GraphList) {
            ProcessLauncher.Instance.LaunchSequence(seq, typeof(EventPinnedMessageDeletedNode), 0, null);
        }
    }
}
