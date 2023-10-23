using FlowGraph;
using FlowGraph.Plugin;
using FlowGraph.Process;
using PusherClient;
using System.Text.Json;
using tech.sublink.KickExtension.Kick.Types;
using tech.sublink.KickExtension.Event;

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

    private void Pusher_Connected(object sender) { }// => PusherConnected?.Invoke(this, EventArgs.Empty);

    private void Pusher_ConnectionStateChanged(object sender, ConnectionState state) { }// =>
    // PusherConnectionStateChanged?.Invoke(this, new PusherConnectionStateChangedArgs { State = state });

    private void Pusher_Disconnected(object sender) { }// => PusherDisconnected?.Invoke(this, EventArgs.Empty);

    private void Pusher_Error(object sender, PusherException error) { }// =>
        //PusherError?.Invoke(this, new PusherErrorArgs
        //{
        //    Exception = error,
        //    Message = error.Message
        //});

    private void Pusher_Subscribed(object sender, Channel channel) { }// =>
        //PusherSubscribed?.Invoke(this, new PusherSubscribedArgs
        //{
        //    SubscriptionCount = channel.SubscriptionCount,
        //    Name = channel.Name
        //});

    private void OnChatMessageEvent(PusherEvent eventData)
    {
        var data = JsonSerializer.Deserialize<ChatMessage>(eventData.Data);

        if (data != null)
            foreach (var seq in GraphDataManager.Instance.GraphList) {
                ProcessLauncher.Instance.LaunchSequence(seq, typeof(EventChatMessageNode), 0, data);
            }
    }

    private void OnGiftedSubscriptionsEvent(PusherEvent eventData)
    {/*
        var data = JsonSerializer.Deserialize<GiftedSubscriptionsEvent>(eventData.Data);

        if (data != null)
            GiftedSubscriptionsEvent?.Invoke(this, new GiftedSubscriptionsEventArgs { Data = data });*/
    }

    private void OnSubscriptionEvent(PusherEvent eventData)
    {/*
        var data = JsonSerializer.Deserialize<SubscriptionEvent>(eventData.Data);

        if (data != null)
            SubscriptionEvent?.Invoke(this, new SubscriptionEventArgs { Data = data });*/
    }

    private void OnStreamHostEvent(PusherEvent eventData)
    {/*
        var data = JsonSerializer.Deserialize<StreamHostEvent>(eventData.Data);

        if (data != null)
            StreamHostEvent?.Invoke(this, new StreamHostEventArgs { Data = data });*/
    }

    private void OnUserBannedEvent(PusherEvent eventData)
    {/*
        var data = JsonSerializer.Deserialize<UserBannedEvent>(eventData.Data);

        if (data != null)
            UserBannedEvent?.Invoke(this, new UserBannedEventArgs { Data = data });*/
    }

    private void OnUserUnbannedEvent(PusherEvent eventData)
    {/*
        var data = JsonSerializer.Deserialize<UserUnbannedEvent>(eventData.Data);

        if (data != null)
            UserUnbannedEvent?.Invoke(this, new UserUnbannedEventArgs { Data = data });*/
    }

    private void OnMessageDeletedEvent(PusherEvent eventData)
    {/*
        var data = JsonSerializer.Deserialize<MessageDeletedEvent>(eventData.Data);

        if (data != null)
            MessageDeletedEvent?.Invoke(this, new MessageDeletedEventArgs { Data = data });*/
    }

    private void OnChatroomClearEvent(PusherEvent eventData)
    {/*
        var data = JsonSerializer.Deserialize<ChatroomClearEvent>(eventData.Data);

        if (data != null)
            ChatroomClearEvent?.Invoke(this, new ChatroomClearEventArgs { Data = data });*/
    }

    private void OnChatroomUpdatedEvent(PusherEvent eventData)
    {/*
        var data = JsonSerializer.Deserialize<ChatroomUpdatedEvent>(eventData.Data);

        if (data != null)
            ChatroomUpdatedEvent?.Invoke(this, new ChatroomUpdatedEventArgs { Data = data });*/
    }

    private void OnPollUpdateEvent(PusherEvent eventData)
    {/*
        var data = JsonSerializer.Deserialize<PollUpdateEvent>(eventData.Data);

        if (data != null)
            PollUpdateEvent?.Invoke(this, new PollUpdateEventArgs { Data = data });*/
    }

    private void OnPollDeleteEvent(PusherEvent eventData) { }// =>
        //PollDeleteEvent?.Invoke(this, EventArgs.Empty);

    private void OnPinnedMessageCreatedEvent(PusherEvent eventData)
    {/*
        var data = JsonSerializer.Deserialize<PinnedMessageCreatedEvent>(eventData.Data);

        if (data != null)
            PinnedMessageCreatedEvent?.Invoke(this, new PinnedMessageCreatedEventArgs { Data = data });*/
    }

    private void OnPinnedMessageDeletedEvent(PusherEvent eventData) { }// =>
        //PinnedMessageDeletedEvent?.Invoke(this, EventArgs.Empty);
}
