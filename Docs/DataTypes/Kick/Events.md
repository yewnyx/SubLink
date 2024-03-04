# SubLink DataTypes Kick Events

[Back To Readme](../../../README.md)  
[Back To Kick DataTypes Index](Index.md)

## ReactToChatMessage

- Parameter type `ChatMessageEvent`
- Boilerplate
```csharp
kick.ReactToChatMessage(async chatMessage => {
    // Your Code
});
```

## ReactToGiftedSubscriptions

- Parameter type `GiftedSubscriptionsEvent`
- Boilerplate
```csharp
kick.ReactToGiftedSubscriptions(async giftedSubs => {
    // Your Code
});
```

## ReactToSubscription

- Parameter type `SubscriptionEvent`
- Boilerplate
```csharp
kick.ReactToSubscription(async sub => {
    // Your Code
});
```

## ReactToStreamHost

- Parameter type `StreamHostEvent`
- Boilerplate
```csharp
kick.ReactToStreamHost(async streamHost => {
    // Your Code
});
```

## ReactToUserBanned

- Parameter type `UserBannedEvent`
- Boilerplate
```csharp
kick.ReactToUserBanned(async banned => {
    // Your Code
});
```

## ReactToUserUnbanned

- Parameter type `UserUnbannedEvent`
- Boilerplate
```csharp
kick.ReactToUserUnbanned(async unbanned => {
    // Your Code
});
```

## ReactToMessageDeleted

- Parameter type `MessageDeletedEvent`
- Boilerplate
```csharp
kick.ReactToMessageDeleted(async deletedMessage => {
    // Your Code
});
```

## ReactToChatroomClear

- Parameter type `ChatroomClearEvent`
- Boilerplate
```csharp
kick.ReactToChatroomClear(async chatroomClear => {
    // Your Code
});
```

## ReactToChatroomUpdated

- Parameter type `ChatroomUpdatedEvent`
- Boilerplate
```csharp
kick.ReactToChatroomUpdated(async chatroomUpdate => {
    // Your Code
});
```

## ReactToPollUpdate

- Parameter type `PollUpdateEvent`
- Boilerplate
```csharp
kick.ReactToPollUpdate(async pollUpdate => {
    // Your Code
});
```

## ReactToPollDelete

- Parameter type `None`
- Boilerplate
```csharp
kick.ReactToPollDelete(async () => {
    // Your Code
});
```

## ReactToPinnedMessageCreated

- Parameter type `PinnedMessageCreatedEvent`
- Boilerplate
```csharp
kick.ReactToPinnedMessageCreated(async pinnedMessage => {
    // Your Code
});
```

## ReactToPinnedMessageDeleted

- Parameter type `None`
- Boilerplate
```csharp
kick.ReactToPinnedMessageDeleted(async () => {
    // Your Code
});
```
