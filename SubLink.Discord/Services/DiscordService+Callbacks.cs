using System;
using System.Threading.Tasks;
using xyz.yewnyx.SubLink.Discord.Client;

namespace xyz.yewnyx.SubLink.Discord.Services;

internal sealed partial class DiscordService {
    private void WireCallbacks() {
        _discord.OnReady += OnReady;
        _discord.OnError += OnError;
        _discord.OnSelectedVoiceChannel += OnSelectedVoiceChannel;
        _discord.OnVoiceSettingsUpdate += OnVoiceSettingsUpdate;
        _discord.OnVoiceStatusUpdate += OnVoiceStatusUpdate;
        _discord.OnGuildStatus += OnGuildStatus;
        _discord.OnGuildCreate += OnGuildCreate;
        _discord.OnChannelCreate += OnChannelCreate;
        _discord.OnVoiceStateCreate += OnVoiceStateCreate;
        _discord.OnVoiceStateUpdate += OnVoiceStateUpdate;
        _discord.OnVoiceStateDelete += OnVoiceStateDelete;
        _discord.OnStartSpeaking += OnStartSpeaking;
        _discord.OnStopSpeaking += OnStopSpeaking;
        _discord.OnMessageCreate += OnMessageCreate;
        _discord.OnMessageUpdate += OnMessageUpdate;
        _discord.OnMessageDelete += OnMessageDelete;
        _discord.OnNotificationCreate += OnNotificationCreate;
        _discord.OnActivityJoin += OnActivityJoin;
        _discord.OnActivitySpectate += OnActivitySpectate;
        _discord.OnActivityJoinRequest += OnActivityJoinRequest;
    }

    private void OnReady(object? sender, EventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnReady: { } callback })
                await callback();
        });

    private void OnError(object? sender, DiscordErrorArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnError: { } callback })
                await callback(e);
        });

    private void OnSelectedVoiceChannel(object? sender, DiscordVoiceChannelIdEventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnSelectedVoiceChannel: { } callback })
                await callback(e.VoiceChannelId);
        });

    private void OnVoiceSettingsUpdate(object? sender, DiscordVoiceSettingsEventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnVoiceSettingsUpdate: { } callback })
                await callback(e);
        });

    private void OnVoiceStatusUpdate(object? sender, DiscordVoiceStatusEventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnVoiceStatusUpdate: { } callback })
                await callback(e);
        });

    private void OnGuildStatus(object? sender, DiscordGuildIdEventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnGuildStatus: { } callback })
                await callback(e.GuildId);
        });

    private void OnGuildCreate(object? sender, DiscordGuildIdEventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnGuildCreate: { } callback })
                await callback(e.GuildId);
        });

    private void OnChannelCreate(object? sender, DiscordChannelEventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnChannelCreate: { } callback })
                await callback(e);
        });

    private void OnVoiceStateCreate(object? sender, DiscordUserIdEventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnVoiceStateCreate: { } callback })
                await callback(e.UserId);
        });

    private void OnVoiceStateUpdate(object? sender, DiscordUserIdEventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnVoiceStateUpdate: { } callback })
                await callback(e.UserId);
        });

    private void OnVoiceStateDelete(object? sender, DiscordUserIdEventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnVoiceStateDelete: { } callback })
                await callback(e.UserId);
        });

    private void OnStartSpeaking(object? sender, DiscordUserIdEventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnStartSpeaking: { } callback })
                await callback(e.UserId);
        });

    private void OnStopSpeaking(object? sender, DiscordUserIdEventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnStopSpeaking: { } callback })
                await callback(e.UserId);
        });

    private void OnMessageCreate(object? sender, DiscordMessageIdEventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnMessageCreate: { } callback })
                await callback(e.MessageId);
        });

    private void OnMessageUpdate(object? sender, DiscordMessageIdEventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnMessageUpdate: { } callback })
                await callback(e.MessageId);
        });

    private void OnMessageDelete(object? sender, DiscordMessageIdEventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnMessageDelete: { } callback })
                await callback(e.MessageId);
        });

    private void OnNotificationCreate(object? sender, DiscordChannelIdEventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnNotificationCreate: { } callback })
                await callback(e.Id);
        });

    private void OnActivityJoin(object? sender, EventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnActivityJoin: { } callback })
                await callback();
        });

    private void OnActivitySpectate(object? sender, EventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnActivitySpectate: { } callback })
                await callback();
        });

    private void OnActivityJoinRequest(object? sender, DiscordUserIdEventArgs e) =>
        Task.Run(async () => {
            if (_rules is DiscordRules { OnActivityJoinRequest: { } callback })
                await callback(e.UserId);
        });
}
