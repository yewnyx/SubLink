using System;

namespace xyz.yewnyx.SubLink.Discord.Client;

public sealed class DiscordErrorArgs : EventArgs
{
    public int ErrorCode { get; set; } = 0;

    public DiscordErrorArgs() { }

    public DiscordErrorArgs(int errCode)
    {
        ErrorCode = errCode;
    }
}

public sealed class DiscordVoiceSettingsEventArgs : EventArgs
{
    public float InputVolume { get; set; } = 0f;
    public float OutputVolume { get; set; } = 0f;

    public DiscordVoiceSettingsEventArgs() { }

    public DiscordVoiceSettingsEventArgs(float inputVolume, float outputVolume)
    {
        InputVolume = inputVolume;
        OutputVolume = outputVolume;
    }
}

public sealed class DiscordVoiceStatusEventArgs : EventArgs
{
    public string State { get; set; } = string.Empty;
    public int StateCode { get; set; } = 0;

    public DiscordVoiceStatusEventArgs() { }

    public DiscordVoiceStatusEventArgs(string state, int stateCode)
    {
        State = state;
        StateCode = stateCode;
    }
}

public sealed class DiscordGuildIdEventArgs : EventArgs
{
    public string GuildId { get; set; } = string.Empty;

    public DiscordGuildIdEventArgs() { }

    public DiscordGuildIdEventArgs(string guildId)
    {
        GuildId = guildId;
    }
}

public sealed class DiscordChannelIdEventArgs : EventArgs
{
    public string ChannelId { get; set; } = string.Empty;

    public DiscordChannelIdEventArgs() { }

    public DiscordChannelIdEventArgs(string channelId)
    {
        ChannelId = channelId;
    }
}

public sealed class DiscordVoiceChannelIdEventArgs : EventArgs
{
    public string VoiceChannelId { get; set; } = string.Empty;

    public DiscordVoiceChannelIdEventArgs() { }

    public DiscordVoiceChannelIdEventArgs(string voicechannelId)
    {
        VoiceChannelId = voicechannelId;
    }
}

public sealed class DiscordUserIdEventArgs : EventArgs
{
    public string UserId { get; set; } = string.Empty;

    public DiscordUserIdEventArgs() { }

    public DiscordUserIdEventArgs(string userId)
    {
        UserId = userId;
    }
}

public sealed class DiscordMessageIdEventArgs : EventArgs
{
    public string MessageId { get; set; } = string.Empty;

    public DiscordMessageIdEventArgs() { }

    public DiscordMessageIdEventArgs(string messageId)
    {
        MessageId = messageId;
    }
}
