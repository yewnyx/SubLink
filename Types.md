## KickBadge

- `string` Type  - Required - Badge type
- `string` Text  - Required - Badge name
- `int`    Count - Optional - The number of months

## KickIdentity

- `string`      Color  - Required - The username color
- `KickBadge[]` Badges - Required - The user's badges, like sub, mod, OG, etc

## KickUser

- `string`       Id       - Required - The Kick user ID
- `string`       Username - Required - The username (Display name equivilant on Twitch)
- `string`       Slug     - Required - The slug (Username equivilant on Twitch)
- `KickIdentity` Identity - Required - The user identity (Basically cosmetics)

## KickUserShort

- `string` Id       - Required - The Kick user ID
- `string` Username - Required - The username (Display name equivilant on Twitch)
- `string` Slug     - Required - The slug (Username equivilant on Twitch)

## MessageInfo

- `string` Id - Required - The message ID

## SlowModeInfo

- `bool` Enabled         - Required - Indicates if the mode is enabled
- `int`  MessageInterval - Required - The amount of seconds between messages

## SubsOnlyModeInfo

- `bool` Enabled - Required - Indicates if the mode is enabled

## FollowOnlyModeInfo

- `bool` Enabled     - Required - Indicates if the mode is enabled
- `int`  MinDuration - Required - The amount of seconds a user must be following

## EmoteModeInfo

- `bool` Enabled - Required - Indicates if the mode is enabled

## AdvBotProtectModeInfo

- `bool` Enabled         - Required - Indicates if the mode is enabled
- `int`  MessageInterval - Required - The amount of messages it takes to trigger the bot protection

## PollOptionInfo

- `uint`   Id    - Required - The poll option ID
- `string` Label - Required - The poll option text
- `int`    Votes - Required - The number of votes for this poll option

## PollInfo

- `string`         Title                 - Required - The poll title text
- `PollOptionInfo` Options               - Required - The poll option information
- `int`            Duration              - Required - The total poll duration in seconds
- `int`            Remaining             - Required - The remaining poll voting time in seconds
- `int`            ResultDisplayDuration - Required - The poll result display duration in seconds

## PinnedMessageInfo

- `string`   Id         - Required - The poll option ID
- `uint`     ChatroomId - Required - The chatroom ID
- `string`   Content    - Required - The message content
- `string`   Type       - Required - The message type
- `string`   CreatedAt  - Required - The creation timestamp of the message
- `KickUser` Sender     - Required - The user who sent the message
- `object`   Metadata   - Required - Unknown additional metadata

## ChatMessageEvent

- `string`   Id         - Required - The message ID
- `uint`     ChatroomId - Required - The chatroom ID
- `string`   Content    - Required - The content of the message
- `string`   Type       - Required - The message type
- `string`   CreatedAt  - Required - The message creation timestamp
- `KickUser` Sender     - Required - The Kick user

## GiftedSubscriptionsEvent

- `string`   Id             - Required - The message ID
- `uint`     ChatroomId     - Required - The chatroom ID
- `string[]` Users          - Required - The gift sub recipients
- `string`   Gifter         - Required - The name of the gifter
- `int`      GetGiftCount() - Util     - The number of gift subs

## SubscriptionEvent

- `string` Id         - Required - The message ID
- `uint`   ChatroomId - Required - The chatroom ID
- `string` Username   - Required - The username
- `uint`   Months     - Required - The cummulative months

## StreamHostEvent

- `uint`   ChatroomId      - Required - The chatroom ID
- `string` OptionalMessage - Optional - The raid message
- `uint`   NumberViewers   - Required - The number of viewers
- `string` HostUsername    - Required - The username of the host target

## UserBannedEvent

- `string`        Id        - Required - The message ID
- `KickUserShort` User      - Required - The banned user
- `KickUserShort` BannedBy  - Required - The moderator who banned the user
- `string`        ExpiresAt - Required - The ban expiry timestamp

## UserUnbannedEvent

- `string`        Id         - Required - The message ID
- `KickUserShort` User       - Required - The unbanned user
- `KickUserShort` UnbannedBy - Required - The moderator who unbanned the user

## MessageDeletedEvent

- `string`      Id      - Required - The message ID
- `MessageInfo` Message - Required - The message ID

## ChatroomClearEvent

- `string` Id - Required - The message ID

## ChatroomUpdatedEvent

- `string`                Id                    - Required - The message ID
- `SlowModeInfo`          SlowMode              - Required - The slow mode settings
- `SubsOnlyModeInfo`      SubscribersMode       - Required - The subs-only mode settings
- `FollowOnlyModeInfo`    FollowersMode         - Required - The followers-only mode settings
- `EmoteModeInfo`         EmotesMode            - Required - The emote-only mode settings
- `AdvBotProtectModeInfo` AdvancedBotProtection - Required - The advanced bot protection mode settings

## PollUpdateEvent

- `PollInfo` Poll - Required - The poll information

## PollDeleteEvent

No data

## PinnedMessageCreatedEvent

- `PinnedMessageInfo` Message  - Required - The pinned message information
- `string`            Duration - Required - The duration of the pin

## PinnedMessageDeletedEvent

No data
