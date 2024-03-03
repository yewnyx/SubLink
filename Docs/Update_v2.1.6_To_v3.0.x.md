# Upgrading your v2.1.6+ files to v3.0.x

This guide should help you upgrade your setup to be compatible with the new release of SubLink. This release is setup in a modular way, so you don't need to mess around with multiple ports and executables. To make this all possible and look representable, we made a couple changes to how things are done.

## settings.json

These have completely changed. We advise you to start the exe without any settings files, this will generate clean template files. Use your original `settings.json` file to update the newly generated settings files with your information.

## SubLink.cs

For each platform you will have to add a new line above the first reaction handler. These are as follows:

### Twitch

You will need to add the following line before the first `twitch.ReactTo` in your script.

```csharp
var twitch = (TwitchRules)rules["Twitch"];
```

Before:

```csharp
#if SUBLINK_TWITCH

logger.Information("Twitch integration enabled");

twitch.ReactToJoinedChannel(async (channel, botUsername) => {
```

After:

```csharp
#if SUBLINK_TWITCH

logger.Information("Twitch integration enabled");
var twitch = (TwitchRules)rules["Twitch"];

twitch.ReactToJoinedChannel(async (channel, botUsername) => {
```

### Kick

You will need to add the following line before the first `kick.ReactTo` in your script.

```csharp
var kick = (KickRules)rules["Kick"];
```

Before:

```csharp
#if SUBLINK_KICK

logger.Information("Kick integration enabled");

kick.ReactToChatMessage(async chatMessage => {
```

After:

```csharp
#if SUBLINK_KICK

logger.Information("Kick integration enabled");
var kick = (KickRules)rules["Kick"];

kick.ReactToChatMessage(async chatMessage => {
```

### StreamPad

You will need to add the following line before the first `streamPad.ReactTo` in your script.

```csharp
var streamPad = (StreamPadRules)rules["StreamPad"];
```

Before:

```csharp
#if SUBLINK_STREAMPAD

logger.Information("StreamPad integration enabled");

streamPad.ReactToControllerValue(async (name, value) => {
```

After:

```csharp
#if SUBLINK_STREAMPAD

logger.Information("StreamPad integration enabled");
var streamPad = (StreamPadRules)rules["StreamPad"];

streamPad.ReactToControllerValue(async (name, value) => {
```

### StreamElements

You will need to add the following line before the first `streamElements.ReactTo` in your script.

```csharp
var streamElements = (StreamElementsRules)rules["StreamElements"];
```

Before:

```csharp
#if SUBLINK_STREAMELEMENTS

logger.Information("StreamElements integration enabled");

streamElements.ReactToTipEvent(async tipInfo => {
```

After:

```csharp
#if SUBLINK_STREAMELEMENTS

logger.Information("StreamElements integration enabled");
var streamElements = (StreamElementsRules)rules["StreamElements"];

streamElements.ReactToTipEvent(async tipInfo => {
```

### Fansly

You will need to add the following line before the first `fansly.ReactTo` in your script.

```csharp
var fansly = (FanslyRules)rules["Fansly"];
```

Before:

```csharp
#if SUBLINK_FANSLY

logger.Information("Fansly integration enabled");

fansly.ReactToChatMessage(async chatMessage => {
```

After:

```csharp
#if SUBLINK_FANSLY

logger.Information("Fansly integration enabled");
var fansly = (FanslyRules)rules["Fansly"];

fansly.ReactToChatMessage(async chatMessage => {
```

You can always look at the [SubLink.cs](https://github.com/yewnyx/SubLink/blob/master/SubLink/SubLink.cs) file in this repository to get an idea of what the new script format looks like;
