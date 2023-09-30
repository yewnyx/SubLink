using System;
using System.Text.Json;
using System.Threading.Tasks;
using PusherClient;
using xyz.yewnyx.SubLink.Kick.Events;

namespace xyz.yewnyx.SubLink.Kick;

internal sealed class KickPusherClient {
    private Pusher? _pusher = null;

    public event EventHandler? PusherConnected;

    public event EventHandler<PusherConnectionStateChangedArgs>? PusherConnectionStateChanged;

    public event EventHandler? PusherDisconnected;

    public event EventHandler<PusherErrorArgs>? PusherError;

    public event EventHandler<PusherSubscribedArgs>? PusherSubscribed;

    public event EventHandler<ChatMessageEventArgs>? ChatMessageEvent;

    public event EventHandler<GiftedSubscriptionsEventArgs>? GiftedSubscriptionsEvent;

    public event EventHandler<SubscriptionEventArgs>? SubscriptionEvent;

    public event EventHandler<StreamHostEventArgs>? StreamHostEvent;

    public event EventHandler<UserBannedEventArgs>? UserBannedEvent;

    public event EventHandler<UserUnbannedEventArgs>? UserUnbannedEvent;

    public event EventHandler<MessageDeletedEventArgs>? MessageDeletedEvent;

    public event EventHandler<ChatroomClearEventArgs>? ChatroomClearEvent;

    public event EventHandler<ChatroomUpdatedEventArgs>? ChatroomUpdatedEvent;

    public event EventHandler<PollUpdateEventArgs>? PollUpdateEvent;

    public event EventHandler<PollDeleteEventArgs>? PollDeleteEvent;

    public event EventHandler<PinnedMessageCreatedEventArgs>? PinnedMessageCreatedEvent;

    public event EventHandler<PinnedMessageDeletedEventArgs>? PinnedMessageDeletedEvent;

    public KickPusherClient() {
    }

    public async Task<bool> ConnectAsync(string key, string cluster, string chatroomId) {
        if (_pusher != null)
            return true;

        _pusher = new Pusher(key, new PusherOptions {
            Cluster = cluster,
            Encrypted = true,
            ClientTimeout = TimeSpan.FromSeconds(120)
        });

        _pusher.Connected += Pusher_Connected;
        _pusher.ConnectionStateChanged += Pusher_ConnectionStateChanged;
        _pusher.Disconnected += Pusher_Disconnected;
        _pusher.Error += Pusher_Error;
        _pusher.Subscribed += Pusher_Subscribed;

        try {
            await _pusher.ConnectAsync();

            // Bind to events
            _pusher.Bind(@"App\Events\ChatMessageEvent",          OnChatMessageEvent);
            _pusher.Bind(@"App\Events\GiftedSubscriptionsEvent",  OnGiftedSubscriptionsEvent);
            _pusher.Bind(@"App\Events\SubscriptionEvent",         OnSubscriptionEvent);
            _pusher.Bind(@"App\Events\StreamHostEvent",           OnStreamHostEvent);
            _pusher.Bind(@"App\Events\UserBannedEvent",           OnUserBannedEvent);
            _pusher.Bind(@"App\Events\UserUnbannedEvent",         OnUserUnbannedEvent);
            _pusher.Bind(@"App\Events\MessageDeletedEvent",       OnMessageDeletedEvent);
            _pusher.Bind(@"App\Events\ChatroomClearEvent",        OnChatroomClearEvent);
            _pusher.Bind(@"App\Events\ChatroomUpdatedEvent",      OnChatroomUpdatedEvent);
            _pusher.Bind(@"App\Events\PollUpdateEvent",           OnPollUpdateEvent);
            _pusher.Bind(@"App\Events\PollDeleteEvent",           OnPollDeleteEvent);
            _pusher.Bind(@"App\Events\PinnedMessageCreatedEvent", OnPinnedMessageCreatedEvent);
            _pusher.Bind(@"App\Events\PinnedMessageDeletedEvent", OnPinnedMessageDeletedEvent);

            await _pusher.SubscribeAsync($"chatrooms.{chatroomId}.v2");
        } catch (Exception) {
            return false;
        }

        return true;
    }

    public async Task DisconnectAsync() {
        if (_pusher == null)
            return;

        _pusher.UnbindAll();
        await _pusher.UnsubscribeAllAsync();
        await _pusher.DisconnectAsync();
        _pusher = null;
    }

    private void Pusher_Connected(object sender) => PusherConnected?.Invoke(this, EventArgs.Empty);

    private void Pusher_ConnectionStateChanged(object sender, ConnectionState state) =>
        PusherConnectionStateChanged?.Invoke(this, new PusherConnectionStateChangedArgs { State = state });

    private void Pusher_Disconnected(object sender) => PusherDisconnected?.Invoke(this, EventArgs.Empty);

    private void Pusher_Error(object sender, PusherException error) =>
        PusherError?.Invoke(this, new PusherErrorArgs {
            Exception = error,
            Message = error.Message
        });

    private void Pusher_Subscribed(object sender, Channel channel) =>
        PusherSubscribed?.Invoke(this, new PusherSubscribedArgs {
            SubscriptionCount = channel.SubscriptionCount,
            Name = channel.Name
        });

    private void OnChatMessageEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<ChatMessageEvent>(eventData.Data);

        if (data != null)
            ChatMessageEvent?.Invoke(this, new ChatMessageEventArgs { Data = data });
    }

    private void OnGiftedSubscriptionsEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<GiftedSubscriptionsEvent>(eventData.Data);

        if (data != null)
            GiftedSubscriptionsEvent?.Invoke(this, new GiftedSubscriptionsEventArgs { Data = data });
    }

    private void OnSubscriptionEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<SubscriptionEvent>(eventData.Data);

        if (data != null)
            SubscriptionEvent?.Invoke(this, new SubscriptionEventArgs { Data = data });
    }

    private void OnStreamHostEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<StreamHostEvent>(eventData.Data);

        if (data != null)
            StreamHostEvent?.Invoke(this, new StreamHostEventArgs { Data = data });
    }

    private void OnUserBannedEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<UserBannedEvent>(eventData.Data);

        if (data != null)
            UserBannedEvent?.Invoke(this, new UserBannedEventArgs { Data = data });
    }

    private void OnUserUnbannedEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<UserUnbannedEvent>(eventData.Data);

        if (data != null)
            UserUnbannedEvent?.Invoke(this, new UserUnbannedEventArgs { Data = data });
    }

    private void OnMessageDeletedEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<MessageDeletedEvent>(eventData.Data);

        if (data != null)
            MessageDeletedEvent?.Invoke(this, new MessageDeletedEventArgs { Data = data });
    }

    private void OnChatroomClearEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<ChatroomClearEvent>(eventData.Data);

        if (data != null)
            ChatroomClearEvent?.Invoke(this, new ChatroomClearEventArgs { Data = data });
    }

    private void OnChatroomUpdatedEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<ChatroomUpdatedEvent>(eventData.Data);

        if (data != null)
            ChatroomUpdatedEvent?.Invoke(this, new ChatroomUpdatedEventArgs { Data = data });
    }

    private void OnPollUpdateEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<PollUpdateEvent>(eventData.Data);

        if (data != null)
            PollUpdateEvent?.Invoke(this, new PollUpdateEventArgs { Data = data });
    }

    private void OnPollDeleteEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<PollDeleteEvent>(eventData.Data);

        if (data != null)
            PollDeleteEvent?.Invoke(this, new PollDeleteEventArgs { Data = data });
    }

    private void OnPinnedMessageCreatedEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<PinnedMessageCreatedEvent>(eventData.Data);

        if (data != null)
            PinnedMessageCreatedEvent?.Invoke(this, new PinnedMessageCreatedEventArgs { Data = data });
    }

    private void OnPinnedMessageDeletedEvent(PusherEvent eventData) {
        var data = JsonSerializer.Deserialize<PinnedMessageDeletedEvent>(eventData.Data);

        if (data != null)
            PinnedMessageDeletedEvent?.Invoke(this, new PinnedMessageDeletedEventArgs { Data = data });
    }
}
