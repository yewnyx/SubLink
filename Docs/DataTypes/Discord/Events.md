# SubLink DataTypes Discord Events

[Back To Readme](../../../README.md)  
[Back To Discord DataTypes Index](Index.md)

## ReactToReady

- Parameter type Nothing
- Boilerplate
```csharp
discord.ReactToReady(async () => {
    // Your Code
});
```

## ReactToError

- Parameter type `int`
- Boilerplate
```csharp
discord.ReactToError(async ErrorCode => {
    // Your Code
});
```

## ReactToSelectedVoiceChannel

- Parameter type `string`
- Boilerplate
```csharp
discord.ReactToSelectedVoiceChannel(async channelId => {
    // Your Code
});
```

## ReactToVoiceSettingsUpdate

- Parameter type `DiscordVoiceSettingsEventArgs`
- Boilerplate
```csharp
discord.ReactToVoiceSettingsUpdate(async voiceSettings => {
    // Your Code
});
```

## ReactToVoiceStatusUpdate

- Parameter type `DiscordVoiceStatusEventArgs`
- Boilerplate
```csharp
discord.ReactToVoiceStatusUpdate(async voiceStatus => {
    // Your Code
});
```

## ReactToGuildStatus

- Parameter type `string`
- Boilerplate
```csharp
discord.ReactToGuildStatus(async guildId => {
    // Your Code
});
```

## ReactToGuildCreate

- Parameter type `string`
- Boilerplate
```csharp
discord.ReactToGuildCreate(async guildId => {
    // Your Code
});
```

## ReactToChannelCreate

- Parameter type `string`
- Boilerplate
```csharp
discord.ReactToChannelCreate(async channelId => {
    // Your Code
});
```

## ReactToVoiceStateCreate

- Parameter type `string`
- Boilerplate
```csharp
discord.ReactToVoiceStateCreate(async userId => {
    // Your Code
});
```

## ReactToVoiceStateUpdate

- Parameter type `string`
- Boilerplate
```csharp
discord.ReactToVoiceStateUpdate(async userId => {
    // Your Code
});
```

## ReactToVoiceStateDelete

- Parameter type `string`
- Boilerplate
```csharp
discord.ReactToVoiceStateDelete(async userId => {
    // Your Code
});
```

## ReactToStartSpeaking

- Parameter type `string`
- Boilerplate
```csharp
discord.ReactToStartSpeaking(async userId => {
    // Your Code
});
```

## ReactToStopSpeaking

- Parameter type `string`
- Boilerplate
```csharp
discord.ReactToStopSpeaking(async userId => {
    // Your Code
});
```

## ReactToMessageCreate

- Parameter type `string`
- Boilerplate
```csharp
discord.ReactToMessageCreate(async messageId => {
    // Your Code
});
```

## ReactToMessageUpdate

- Parameter type `string`
- Boilerplate
```csharp
discord.ReactToMessageUpdate(async messageId => {
    // Your Code
});
```

## ReactToMessageDelete

- Parameter type `string`
- Boilerplate
```csharp
discord.ReactToMessageDelete(async messageId => {
    // Your Code
});
```

## ReactToNotificationCreate

- Parameter type `string`
- Boilerplate
```csharp
discord.ReactToNotificationCreate(async channelId => {
    // Your Code
});
```

## ReactToActivityJoin

- Parameter type Nothing
- Boilerplate
```csharp
discord.ReactToActivityJoin(async () => {
    // Your Code
});
```

## ReactToActivitySpectate

- Parameter type Nothing
- Boilerplate
```csharp
discord.ReactToActivitySpectate(async () => {
    // Your Code
});
```

## ReactToActivityJoinRequest

- Parameter type `string`
- Boilerplate
```csharp
discord.ReactToActivityJoinRequest(async userId => {
    // Your Code
});
```
