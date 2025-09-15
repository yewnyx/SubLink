# SubLink Setup Discord

[Back To Readme](../../README.md)

## Setup

1. On the first run, the application will create a `Settings/Discord.json` file.
2. Set the `Settings/Fansly.json` file's `Enabled` setting to `true`. (All lowercase!)
3. Retrieve your Client ID and Secret, using your browser.
   1. Go to the Discord developer portal: https://discord.com/developers/applications/
   2. Create a new Application by clicking on `New Application`. Give it a creative name.
   3. Click on `OAuth2`.
   4. Copy the `Client ID` and set it as the `Settings/Discord.json` file's `ClientID` setting.
   5. Click `Reset Secret` under `Client Secret` and set it's value as the `Settings/Discord.json` file's `ClientSecret` setting.
4. On the second run, the application will automatically connect to Discords's real-time API and start receiving events.

## Config Template

```json
{
  "Discord": {
    "Enabled": true,
    "ClientID": "123_your_client_id_789",
    "ClientSecret": "someSuperSecretClientSecret",
    "DefaultGuildId": "",
    "DefaultChannelId": ""
  }
}
```
