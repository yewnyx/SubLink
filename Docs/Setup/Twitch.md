# SubLink Setup Twitch

[Back To Readme](https://github.com/yewnyx/SubLink/blob/master/README.md)

## Setup

1. On the first run, the application will create a `Settings/Twitch.json` file.
2. Add your `clientid` and `clientsecret` obtained from your Twitch Developer account (or provided to you by CatGirlEddie to the `Settings/Twitch.json` file).  
If you prefer setting this up yourself, see the [Twitch Auth](#twitch-auth) section
3. On the second run, the application will automatically authorize through Twitch and save an access token and refresh token to `Settings/Twitch.json`.

## Config Template

```json
{
  "Twitch": {
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "AccessToken": "",
    "RefreshToken": "",
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
  }
}
```
