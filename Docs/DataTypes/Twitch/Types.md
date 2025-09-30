# SubLink DataTypes Twitch Types

[Back To Readme](../../../README.md)  
[Back To Twitch DataTypes Index](Index.md)

## Emote

- `string` Id - Required - The emote's ID
- `string` Name - Required - The emote's name
- `int` StartIndex - Required - Start index in the message string
- `int` EndIndex - Required - End index in the message string
- `string` ImageUrl - Required - The emote's image URL

## EmoteSet

- `List<Emote>` Emotes - Required - The emotes in this set
- `string` RawEmoteSetString - Required - The emotes as a raw data string

## UserType

This is an enum with the following options:

- Viewer
- Moderator
- GlobalModerator
- Broadcaster
- Admin
- Staff

## ChatReply

- `string` ParentDisplayName - Required - The parent message's user display name
- `string` ParentMsgBody - Required - The parent message's content
- `string` ParentMsgId - Required - The parent message's ID
- `string` ParentUserId - Required - The parent message's user ID
- `string` ParentUserLogin - Required - The parent message's username

## BadgeColor

This is an enum with the following options:

- Red = 10000
- Blue = 5000
- Green = 1000
- Purple = 100
- Gray = 1

## CheerBadge

- `int` CheerAmount - Required - The amount the user has cheered
- `BadgeColor` Color - Required - The colour of the user's cheer badge

## BitsEmote

- `string` Id - Required - The ID that uniquely identifies this emote
- `string` EmoteSetId - Required - The ID that identifies the emote set that the emote belongs to
- `string` OwnerId - Required - The ID of the broadcaster who owns the emote
- `string[]` Format - Required - The formats that the emote is available in; Possible values: animated, static

## BitsCheermote

- `string` Prefix - Required - The name portion of the Cheermote string that you use in chat to cheer Bits
- `int` Bits - Required - The amount of Bits cheered
- `int` Tier - Required - The tier level of the cheermote

## BitsMessageFragments

- `string` Text - Required - The message text in fragment
- `string` Type - Required - The type of message fragment; Possible values: text, cheermote, emote
- `BitsEmote` Emote - Required if `emote` - The metadata pertaining to the emote
- `BitsCheermote` Cheermote - Required if `cheermote` - The metadata pertaining to the cheermote

## BitsMessage

- `string` Text - Required - The chat message in plain text
- `BitsMessageFragments[]` Fragments - Required - The ordered list of chat message fragments

## PowerUpEmote

- `string` Id - Required - The ID that uniquely identifies this emote
- `string` Name - Required - The human readable emote token

## PowerUp

- `string` Type - Required - Possible values are: message_effect, celebration, gigantify_an_emote
- `PowerUpEmote` Emote - Required - Emote associated with the reward
- `string` MessageEffectId - Optional - The ID of the message effect

## Noisy

This is an enum with the following options:

- NotSet
- True
- False

## HypeTrainContribution

- `string` UserId - Required - The contributing user's ID
- `string` UserName - Required - The contributing user's display name
- `string` UserLogin - Required - The contributing user's username
- `string` Type - Required - The contribution type
- `int` Total - Required - The total contribution value

## SharedTrainParticipants

- `int` BroadcasterUserId - Required - The shared hypetrain broadcaster user ID
- `int` BroadcasterUserLogin - Required - The shared hypetrain broadcaster username
- `int` BroadcasterUserName - Required - The shared hypetrain broadcaster display name

## RedemptionReward

- `string` Id - Required - The redemption's ID
- `string` Title - Required - The redemption's title
- `string` Cost - Required - The redemption's channel point cost
- `string` Prompt - Required - The redemption's prompt

## PollChoice

- `string` Id - Required - The poll option's ID
- `string` Title - Required - The poll option's title
- `string` BitsVotes - Optional - The poll option's bits boosted vote count
- `string` ChannelPointsVotes - Optional - The poll option's channel points boosted vote count
- `string` Votes - Optional - The poll option's vote count

## PollVotingSettings

- `bool` IsEnabled - Required - Indicates if this additional vote option is enabled
- `int` AmountPerVote - Required - Abount of bits/points per additional vote

## Predictor

- `string` UserId - Required - The predicting user's ID
- `string` UserLogin - Required - The predicting user's username
- `string` UserName - Required - The predicting user's display name
- `int?` ChannelPointsWon - Optional - The mount of channel points the predicting user won
- `int` ChannelPointsUsed - Required - The mount of channel points the predicting user used

## PredictionOutcomes

- `string` Id - Required - The prediction outcome's ID
- `string` Title - Required - The prediction outcome's title
- `string` Color - Required - The prediction outcome's colour
- `int?` Users - Optional - The prediction outcome's user count
- `int?` ChannelPoints - Optional - The prediction outcome's channel point coint
- `Predictor[]` TopPredictors - Required - The prediction outcome's top contributors

## SubscriptionMessageEmote

- `int` Begin - Required - The emote's starting character index in the message
- `int` End - Required - The emote's ending character index in the message
- `string` Id - Required - The emote's ID

## SubscriptionMessage

- `string` Text - Required - The message's text
- `SubscriptionMessageEmote[]` Emotes - Optional - The message's emotes

## ChatMessage

- `List<KeyValuePair<string, string>>` Badges - Required - The sending user's global and channel badges
- `string` BotUsername - Required - This application's bot user name
- `Color` Color - Required - The sending user's display name colour
- `string` ColorHex - Required - The sending user's display name colour in hexadecimal
- `string` DisplayName - Required - The sending user's display name
- `EmoteSet` EmoteSet - Optional - The emotes attached to the message
- `bool` IsTurbo - Required - Indication if the sending user is a Twitch Turbo member
- `string` UserId - Required - The sending user's ID
- `string` Username - Required - The sending user's username
- `UserType` UserType - Required - The message's user type
- `string` RawIrcMessage - Required - The raw IRC message data
- `List<KeyValuePair<string, string>>` BadgeInfo - Required - User badge information
- `int` Bits - Optional - The amount of bits attached to the message
- `double` BitsInDollars - Optional - The dollar amount of bits attached to the message
- `string` Channel - Required - The channel the message was sent in
- `CheerBadge` CheerBadge - Optional - The cheer badge attached to the message
- `string` CustomRewardId - Optional - The custom channel point reward ID attached to the message
- `string` EmoteReplacedMessage - Required - The message with emote tags replaced with the emote images
- `string` Id - Required - The message's ID
- `bool` IsBroadcaster - Required -  Indication if the message was sent by the broadcaster
- `bool` IsFirstMessage - Required -  Indication if the message was the user's first ever message in this channel
- `bool` IsHighlighted - Required -  Indication if the message was sent using the highlight redeem/option
- `bool` IsMe - Required -  Indication if the message was sent by this app's user account
- `bool` IsModerator - Required -  Indication if the message was sent by a channel moderator
- `bool` IsSkippingSubMode - Required -  Indication if the message was sent using the SubMode bypass redeem
- `bool` IsSubscriber - Required -  Indication if the message was sent by a channel sub
- `bool` IsVip - Required -  Indication if the message was sent by a channel VIP
- `bool` IsStaff - Required -  Indication if the message was sent by a Twitch staff member
- `bool` IsPartner - Required - Indication if the message was sent by a partnered user
- `string` Message - Required - The message's content
- `Noisy` Noisy - Required - Indication if the message in "noisy" for nuisance detection
- `string` RoomId - Required - The chatchoom ID the message was sent in
- `int` SubscribedMonthCount - Required - The number of months the message's user is currently subscribed for
- `string` TmiSentTs - Required - The message's timestamp
- `ChatReply` ChatReply - Optional - The message that this one was replying to

## ChannelBitsUse

- `string` BroadcasterUserId - Required - The receiving broadcaster's user ID
- `string` BroadcasterUserLogin - Required - The receiving broadcaster's username
- `string` BroadcasterUserName - Required - The receiving broadcaster's display name
- `string` UserId - Optional - The tipping user's ID
- `string` UserLogin - Optional - The tipping user's username
- `string` UserName - Optional - The tipping user's display name
- `int` Bits - Required - The number of bits
- `string` Type - Required - The tip type; Possible values are: cheer, power_up
- `BitsMessage` Message - Required if `cheer` - The cheer message info
- `PowerUp` PowerUp - Required if `power_up` - The power-up information

## ChannelCheer

- `bool` IsAnonymous - Required - Indication if the cheer was sent anonymously
- `string` UserId - Optional - The tipping user's ID
- `string` UserName - Optional - The tipping user's display name
- `string` UserLogin - Optional - The tipping user's username
- `string` BroadcasterUserId - Required - The receiving broadcaster's user ID
- `string` BroadcasterUserName - Required - The receiving broadcaster's display name
- `string` BroadcasterUserLogin - Required - The receiving broadcaster's username
- `string` Message - Optional - The cheer's message
- `int` Bits - Required - The number of bits

## ChannelFollow

- `string` UserId - Required - The following user's ID
- `string` UserName - Required - The following user's display name
- `string` UserLogin - Required - The following user's username
- `string` BroadcasterUserId - Required - The receiving broadcaster's user ID
- `string` BroadcasterUserName - Required - The receiving broadcaster's display name
- `string` BroadcasterUserLogin - Required - The receiving broadcaster's username
- `DateTimeOffset` FollowedAt - Required - The offset when the user followed (Unsure, kinda usseless info)

## HypeTrainBeginV2

- `string` BroadcasterUserId - Required - The hypetrain target broadcaster's ID
- `string` BroadcasterUserLogin - Required - The hypetrain target broadcaster's username
- `string` BroadcasterUserName - Required - The hypetrain target broadcaster's display name
- `int` Total - Required - The current contribution total value
- `int` Progress - Required - The hypetrain's current level progress
- `int` Goal - Required - The hypetrain's current level goal
- `HypeTrainContribution[]` TopContributions - Required - The list of top contributions
- `DateTimeOffset` StartedAt - Required - The start time offset
- `int` Level - Required - The hypetrain's current level
- `int` AllTimeHighLevel - Required - The hypetrain's all-time highest level
- `int` AllTimeHighTotal - Required - The hypetrain's all-time highest total value
- `SharedTrainParticipants[]` SharedTrainParticipants - Optional - The hypertrain's shared participants
- `DateTimeOffset` StartedAt - Required - The start time offset
- `DateTimeOffset` ExpiresAt - Required - The hypetrain's expiry offset
- `string` Type - Required - The hypetrain's type (Normal vs golden kappa)
- `bool` IsSharedTrain - Required - Wether the hypertrain is shared or not

## HypeTrainEndV2

- `string` BroadcasterUserId - Required - The hypetrain target broadcaster's ID
- `string` BroadcasterUserLogin - Required - The hypetrain target broadcaster's username
- `string` BroadcasterUserName - Required - The hypetrain target broadcaster's display name
- `int` Total - Required - The current contribution total value
- `HypeTrainContribution[]` TopContributions - Required - The list of top contributions
- `int` Level - Required - The hypetrain's current level
- `SharedTrainParticipants[]` SharedTrainParticipants - Optional - The hypertrain's shared participants
- `DateTimeOffset` StartedAt - Required - The start time offset
- `DateTimeOffset` CooldownEndsAt - Required - The hypetrain cooldown offset
- `DateTimeOffset` EndedAt - Required - The offset at which the hypetrain ended
- `string` Type - Required - The hypetrain's type (Normal vs golden kappa)
- `bool` IsSharedTrain - Required - Wether the hypertrain is shared or not

## HypeTrainProgressV2

- `string` BroadcasterUserId - Required - The hypetrain target broadcaster's ID
- `string` BroadcasterUserLogin - Required - The hypetrain target broadcaster's username
- `string` BroadcasterUserName - Required - The hypetrain target broadcaster's display name
- `int` Total - Required - The current contribution total value
- `int` Progress - Required - The hypetrain's current level progress
- `int` Goal - Required - The hypetrain's current level goal
- `HypeTrainContribution[]` TopContributions - Required - The list of top contributions
- `int` Level - Required - The hypetrain's current level
- `SharedTrainParticipants[]` SharedTrainParticipants - Optional - The hypertrain's shared participants
- `DateTimeOffset` StartedAt - Required - The start time offset
- `DateTimeOffset` ExpiresAt - Required - The hypetrain's expiry offset
- `string` Type - Required - The hypetrain's type (Normal vs golden kappa)
- `bool` IsSharedTrain - Required - Wether the hypertrain is shared or not

## ChannelPointsCustomRewardRedemption

- `string` Id - Required - The channel point redemption's ID
- `string` BroadcasterUserId - Required - The receiving broadcaster's user ID
- `string` BroadcasterUserName - Required - The receiving broadcaster's display name
- `string` BroadcasterUserLogin - Required - The receiving broadcaster's username
- `string` UserId - Required - The redeeming user's ID
- `string` UserName - Required - The redeeming user's display name
- `string` UserLogin - Required - The redeeming user's username
- `string` UserInput - Required - The redeeming user's prompt response
- `string` Status - Required - The channel point redemption's status
- `RedemptionReward` Reward - Required - The reward attached to the channel point redemption
- `DateTimeOffset` RedeemedAt - Required - The redemption offset

## ChannelPollBegin

- `string` Id - Required - The poll's ID
- `string` BroadcasterUserId - Required - The poll's broadcaster ID
- `string` BroadcasterUserLogin - Required - The poll's broadcaster username
- `string` BroadcasterUserName - Required - The poll's broadcaster display name
- `string` Title - Required - The poll's title
- `PollChoice[]` Choices - Required - The poll's choices
- `PollVotingSettings` BitsVoting - Required - The poll's settings for bits boosted voting
- `PollVotingSettings` ChannelPointsVoting - Required - The poll's settings for channel points boosted voting
- `DateTimeOffset` StartedAt - Required - The poll's start time offset
- `DateTimeOffset` EndsAt - Required - The poll's end time offset

## ChannelPollEnd

- `string` Id - Required - The poll's ID
- `string` BroadcasterUserId - Required - The poll's broadcaster ID
- `string` BroadcasterUserLogin - Required - The poll's broadcaster username
- `string` BroadcasterUserName - Required - The poll's broadcaster display name
- `string` Title - Required - The poll's title
- `PollChoice[]` Choices - Required - The poll's choices
- `PollVotingSettings` BitsVoting - Required - The poll's settings for bits boosted voting
- `PollVotingSettings` ChannelPointsVoting - Required - The poll's settings for channel points boosted voting
- `DateTimeOffset` StartedAt - Required - The poll's start time offset
- `string` Status - Required - The poll's ending status
- `DateTimeOffset` EndedAt - Required - The poll's end time offset

## ChannelPollProgress

- `string` Id - Required - The poll's ID
- `string` BroadcasterUserId - Required - The poll's broadcaster ID
- `string` BroadcasterUserLogin - Required - The poll's broadcaster username
- `string` BroadcasterUserName - Required - The poll's broadcaster display name
- `string` Title - Required - The poll's title
- `PollChoice[]` Choices - Required - The poll's choices
- `PollVotingSettings` BitsVoting - Required - The poll's settings for bits boosted voting
- `PollVotingSettings` ChannelPointsVoting - Required - The poll's settings for channel points boosted voting
- `DateTimeOffset` StartedAt - Required - The poll's start time offset
- `DateTimeOffset` EndsAt - Required - The poll's end time offset

## ChannelPredictionBegin

- `string` Id - Required - The prediction's ID
- `string` BroadcasterUserId - Required - The prediction's broadcaster ID
- `string` BroadcasterUserLogin - Required - The prediction's broadcaster username
- `string` BroadcasterUserName - Required - The prediction's display name
- `string` Title - Required - The prediction's title
- `PredictionOutcomes]` Outcomes - Required - The prediction's outcomes
- `DateTimeOffset` StartedAt - Required - The prediction's start time offset
- `DateTimeOffset` LocksAt - Required - The prediction's locking time offset

## ChannelPredictionEnd

- `string` Id - Required - The prediction's ID
- `string` BroadcasterUserId - Required - The prediction's broadcaster ID
- `string` BroadcasterUserLogin - Required - The prediction's broadcaster username
- `string` BroadcasterUserName - Required - The prediction's display name
- `string` Title - Required - The prediction's title
- `PredictionOutcomes]` Outcomes - Required - The prediction's outcomes
- `DateTimeOffset` StartedAt - Required - The prediction's start time offset
- `string` WinningOutcomeId - Required - The prediction's winning outcome ID
- `string` Status - Required - The prediction's status
- `DateTimeOffset` EndedAt - Required - The prediction's ending time offset

## ChannelPredictionLock

- `string` Id - Required - The prediction's ID
- `string` BroadcasterUserId - Required - The prediction's broadcaster ID
- `string` BroadcasterUserLogin - Required - The prediction's broadcaster username
- `string` BroadcasterUserName - Required - The prediction's display name
- `string` Title - Required - The prediction's title
- `PredictionOutcomes]` Outcomes - Required - The prediction's outcomes
- `DateTimeOffset` StartedAt - Required - The prediction's start time offset
- `DateTimeOffset` LockedAt - Required - The prediction's locking time offset

## ChannelPredictionProgress

- `string` Id - Required - The prediction's ID
- `string` BroadcasterUserId - Required - The prediction's broadcaster ID
- `string` BroadcasterUserLogin - Required - The prediction's broadcaster username
- `string` BroadcasterUserName - Required - The prediction's display name
- `string` Title - Required - The prediction's title
- `PredictionOutcomes]` Outcomes - Required - The prediction's outcomes
- `DateTimeOffset` StartedAt - Required - The prediction's start time offset
- `DateTimeOffset` LocksAt - Required - The prediction's locking time offset

## ChannelRaid

- `string` FromBroadcasterUserId - Required - The source broadcaster's ID
- `string` FromBroadcasterUserName - Required - The source broadcaster's display name
- `string` FromBroadcasterUserLogin - Required - The source broadcaster's username
- `string` ToBroadcasterUserId - Required - The target broadcaster's ID
- `string` ToBroadcasterUserName - Required - The target broadcaster's display name
- `string` ToBroadcasterUserLogin - Required - The target broadcaster's username
- `int` Viewers - Required - The number of viewers in the raid

## ChannelSubscribe

- `string` UserId - Required - The subscribing user's  ID
- `string` UserName - Required - The subscribing user's  display name
- `string` UserLogin - Required - The subscribing user's username
- `string` BroadcasterUserId - Required - The target broadcaster's ID
- `string` BroadcasterUserName - Required - The target broadcaster's display name
- `string` BroadcasterUserLogin - Required - The target broadcaster's username
- `string` Tier - Required - The subscription tier
- `bool` IsGift - Required - Indicates if the subscription is a gifted one

## ChannelSubscriptionEnd

- `string` UserId - Required - The subscribing user's  ID
- `string` UserName - Required - The subscribing user's  display name
- `string` UserLogin - Required - The subscribing user's username
- `string` BroadcasterUserId - Required - The target broadcaster's ID
- `string` BroadcasterUserName - Required - The target broadcaster's display name
- `string` BroadcasterUserLogin - Required - The target broadcaster's username
- `string` Tier - Required - The subscription tier
- `bool` IsGift - Required - Indicates if the subscription is a gifted one

## ChannelSubscriptionGift

- `string` UserId - Optional - The gifting user's  ID
- `string` UserName - Optional - The gifting user's  display name
- `string` UserLogin - Optional - The gifting user's username
- `string` BroadcasterUserId - Required - The target broadcaster's ID
- `string` BroadcasterUserName - Required - The target broadcaster's display name
- `string` BroadcasterUserLogin - Required - The target broadcaster's username
- `int` Total - Required - The number of gifted subs
- `string` Tier - Required - The gifted subscription tier
- `int?` CumulativeTotal - Optional - The cumulative number of gifted subs by this user
- `bool` IsAnonymous - Required - Indicates if the gifted subs were done anonymously

## ChannelSubscriptionMessage

- `string` UserId - Optional - The subscribing user's  ID
- `string` UserName - Optional - The subscribing user's  display name
- `string` UserLogin - Optional - The subscribing user's username
- `string` BroadcasterUserId - Required - The target broadcaster's ID
- `string` BroadcasterUserName - Required - The target broadcaster's display name
- `string` BroadcasterUserLogin - Required - The target broadcaster's username
- `string` Tier - Required - The subscription tier
- `SubscriptionMessage` Message - Required - The subscription renewal message
- `int` CumulativeMonths - Required - The subscribing user's total subscribed month count
- `int?` StreakMonths - Optional - The subscribing user's current month streak
- `int` DurationMonths - Required - The duration of the subscription renewal in months

## ChannelUpdate

- `string` BroadcasterUserId - Required - The broadcaster's ID
- `string` BroadcasterUserName - Required - The broadcaster's display name
- `string` BroadcasterUserLogin - Required - The broadcaster's username
- `string` Title - Required - The stream's title
- `string` Language - Required - The stream's language
- `string` CategoryId - Required - The stream's category ID
- `string` CategoryName - Required - The stream's category name
- `bool` IsMature - Required - Indicates if the stream is intended for mature audiences

## StreamOffline

- `string` BroadcasterUserId - Required - The broadcaster's ID
- `string` BroadcasterUserName - Required - The broadcaster's display name
- `string` BroadcasterUserLogin - Required - The broadcaster's username

## StreamOnline

- `string` Id - Required - The stream's ID
- `string` BroadcasterUserId - Required - The broadcaster's ID
- `string` BroadcasterUserName - Required - The broadcaster's display name
- `string` BroadcasterUserLogin - Required - The broadcaster's username
- `string` Type - Required - The stream's type
- `DateTimeOffset` StartedAt - Required - The stream's start time offset
