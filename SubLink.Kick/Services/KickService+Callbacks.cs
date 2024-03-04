using System;
using System.Threading.Tasks;
using xyz.yewnyx.SubLink.Kick.KickClient;

namespace xyz.yewnyx.SubLink.Kick.Services;

internal sealed partial class KickService {
    private void WireCallbacks() {
        _kick.ChatMessageEvent += OnChatMessageEvent;
        _kick.GiftedSubscriptionsEvent += OnGiftedSubscriptionsEvent;
        _kick.SubscriptionEvent += OnSubscriptionEvent;
        _kick.StreamHostEvent += OnStreamHostEvent;
        _kick.UserBannedEvent += OnUserBannedEvent;
        _kick.UserUnbannedEvent += OnUserUnbannedEvent;
        _kick.MessageDeletedEvent += OnMessageDeletedEvent;
        _kick.ChatroomClearEvent += OnChatroomClearEvent;
        _kick.ChatroomUpdatedEvent += OnChatroomUpdatedEvent;
        _kick.PollUpdateEvent += OnPollUpdateEvent;
        _kick.PollDeleteEvent += OnPollDeleteEvent;
        _kick.PinnedMessageCreatedEvent += OnPinnedMessageCreatedEvent;
        _kick.PinnedMessageDeletedEvent += OnPinnedMessageDeletedEvent;
    }

    private void OnChatMessageEvent(object? sender, ChatMessageEventArgs e) {
        Task.Run(async () => {
            if (_rules is KickRules { OnChatMessage: { } callback })
                await callback(e.Data);

            //_logger.Debug("sender: {Sender} event: {@E}", sender, e);
        });
    }

    private void OnGiftedSubscriptionsEvent(object? sender, GiftedSubscriptionsEventArgs e) {
        Task.Run(async () => {
            if (_rules is KickRules { OnGiftedSubscriptions: { } callback })
                await callback(e.Data);

            _logger.Debug("sender: {Sender} event: {@E}", sender, e);
        });
    }

    private void OnSubscriptionEvent(object? sender, SubscriptionEventArgs e) {
        Task.Run(async () => {
            if (_rules is KickRules { OnSubscription: { } callback })
                await callback(e.Data);

            _logger.Debug("sender: {Sender} event: {@E}", sender, e);
        });
    }

    private void OnStreamHostEvent(object? sender, StreamHostEventArgs e) {
        Task.Run(async () => {
            if (_rules is KickRules { OnStreamHost: { } callback })
                await callback(e.Data);

            _logger.Debug("sender: {Sender} event: {@E}", sender, e);
        });
    }

    private void OnUserBannedEvent(object? sender, UserBannedEventArgs e)  {
        Task.Run(async () => {
            if (_rules is KickRules { OnUserBanned: { } callback })
                await callback(e.Data);

            _logger.Debug("sender: {Sender} event: {@E}", sender, e);
        });
    }

    private void OnUserUnbannedEvent(object? sender, UserUnbannedEventArgs e) {
        Task.Run(async () => {
            if (_rules is KickRules { OnUserUnbanned: { } callback })
                await callback(e.Data);

            _logger.Debug("sender: {Sender} event: {@E}", sender, e);
        });
    }

    private void OnMessageDeletedEvent(object? sender, MessageDeletedEventArgs e) {
        Task.Run(async () => {
            if (_rules is KickRules { OnMessageDeleted: { } callback })
                await callback(e.Data);

            _logger.Debug("sender: {Sender} event: {@E}", sender, e);
        });
    }

    private void OnChatroomClearEvent(object? sender, ChatroomClearEventArgs e) {
        Task.Run(async () => {
            if (_rules is KickRules { OnChatroomClear: { } callback })
                await callback(e.Data);

            _logger.Debug("sender: {Sender} event: {@E}", sender, e);
        });
    }

    private void OnChatroomUpdatedEvent(object? sender, ChatroomUpdatedEventArgs e) {
        Task.Run(async () => {
            if (_rules is KickRules { OnChatroomUpdated: { } callback })
                await callback(e.Data);

            _logger.Debug("sender: {Sender} event: {@E}", sender, e);
        });
    }

    private void OnPollUpdateEvent(object? sender, PollUpdateEventArgs e) {
        Task.Run(async () => {
            if (_rules is KickRules { OnPollUpdate: { } callback })
                await callback(e.Data);

            //_logger.Debug("sender: {Sender} event: {@E}", sender, e);
        });
    }

    private void OnPollDeleteEvent(object? sender, EventArgs e) {
        Task.Run(async () => {
            if (_rules is KickRules { OnPollDelete: { } callback })
                await callback(e);

            _logger.Debug("sender: {Sender} event: {@E}", sender, e);
        });
    }

    private void OnPinnedMessageCreatedEvent(object? sender, PinnedMessageCreatedEventArgs e) {
        Task.Run(async () => {
            if (_rules is KickRules { OnPinnedMessageCreated: { } callback })
                await callback(e.Data);

            _logger.Debug("sender: {Sender} event: {@E}", sender, e);
        });
    }

    private void OnPinnedMessageDeletedEvent(object? sender, EventArgs e) {
        Task.Run(async () => {
            if (_rules is KickRules { OnPinnedMessageDeleted: { } callback })
                await callback(e);

            _logger.Debug("sender: {Sender} event: {@E}", sender, e);
        });
    }
}
