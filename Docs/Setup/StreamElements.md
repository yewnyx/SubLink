# SubLink Setup StreamElements

[Back To Readme](https://github.com/yewnyx/SubLink/blob/master/README.md)

## Setup

1. On the first run, the application will create a `Settings/StreamElements.json` file.
2. Retrieve your JWT Token, using your browser, from the following URL: https://streamelements.com/dashboard/account/channels
3. Add the `JWT Token` in your browser to the `Settings/StreamElements.json` file's `JWTToken` setting.
4. On the second run, the application will automatically connect to StreamElements' real-time API and start receiving events.

## Config Template

```json
{
  "StreamElements": {
    "JWTToken": "your-jwt-token-that-is-super-long"
  }
}
```
