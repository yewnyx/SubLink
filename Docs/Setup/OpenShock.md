# SubLink Setup OpenShock

[Back To Readme](../../README.md)

## Setup

1. On the first run, the application will create a `Settings/OBS.json` file.
2. Create a new API token
   1. Open the token dashboard [Token Dashboard](https://openshock.app/#/dashboard/tokens)
   2. Click the `+` button on the bottom-right
   3. Give the token a name like `SubLink Token`
   4. Copy and store the `Code` value in the pop-up. !! Note: You can only see this token (code) once !!
3. Set the `Enabled` setting to `true` in `Settings/OpenShock.json`.
4. Set the `Token` setting to the `Code` value you stored earlier in `Settings/OpenShock.json`.
5. Optional: Set the `Server` value to your own server address in `Settings/OpenShock.json`.
6. On the second run, the application will automatically connect to the OpenShock API.

## Config Template

```json
{
  "OpenShock": {
    "Enabled": false,
    "Server": "https://api.openshock.app",
    "Token": "your-token"
  }
}
```

## Getting your device IDs:

You can either run the snippet: [Shocker Snippets](../Actions/OpenShock/Index.md#Write-the-owned-shockers-to-the-console-and-log-file)  
Or you can follow these steps:

1. Open the shockers dashboard: [Shockers Dashboard](https://openshock.app/#/dashboard/shockers/own)
2. Click the 3 dots on the right of the shocker block, then `Edit`
3. Write down the `ID` value in the pop-up and click cancel
4. Repeat steps `2` and `3` for each tracker
