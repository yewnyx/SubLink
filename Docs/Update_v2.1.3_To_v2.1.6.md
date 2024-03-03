# Upgrading your v2.1.3 files to v2.1.6

If you don't plan on using Kick at all you can safely skip this document. We made sure not to break any existing features, settings or scripts.  
This document only applies to people who wish to use the Kick integration or wish to future-proof themselves.

## settings.json

You will have to ask us for the PusherKey and PusherCluster.  
Once you have this, you can add the following snippet to your settings file, right above `"Discord": {`

```json
  "Kick": {
    "PusherKey": "",
    "PusherCluster": "",
    "ChatroomId": ""
  },
```

The result should look something like the following:

```json
{
  "Twitch": {
    "ClientId": "...",
    "ClientSecret": "...",
    "AccessToken": "...",
    "RefreshToken": "...",
    "Scopes": [
      "bits:read",
      "channel:manage:polls",
      "channel:manage:redemptions",
      "channel:read:hype_train",
      "channel:read:polls",
      "channel:read:redemptions",
      "channel:read:subscriptions",
      "channel:read:vips",
      "chat:edit",
      "chat:read"
    ]
  },
  "Kick": {
    "PusherKey": "...",
    "PusherCluster": "...",
    "ChatroomId": "..."
  },
  "Discord": {
    "Webhook": "..."
  },
  "SubLink": {
    "Discriminator": 123
  }
}
```

## SubLink.cs

Add the following snippet right after `var notifier = new XSNotifier();`

```csharp

#if SUBLINK_TWITCH
```

This should result in the following:

```csharp
using Serilog;
using System;
using System.Threading.Channels;

var notifier = new XSNotifier();

#if SUBLINK_TWITCH

logger.Information("Twitch integration enabled");
```

Add the following at the very bottom of your `SubLink.cs` file:

```csharp

#endif
#if SUBLINK_KICK

logger.Information("Kick integration enabled");

#endif
```

You can always look at the [SubLink.cs](https://github.com/yewnyx/SubLink/blob/858ecd1dad79167f7cbcdbf3c3b954304366b33b/SubLinkCommon/SubLink.cs#L118) file in this repository to get an idea of how to react to Kick events.
