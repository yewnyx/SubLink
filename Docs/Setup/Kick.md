# SubLink Setup Kick

[Back To Readme](../../README.md)

## Setup

1. On the first run, the application will create a `Settings/Kick.json` file.
2. Retrieve your Chatroom ID, using an **In-private**/**Incognito** browser, from the following URL: https://kick.com/api/v2/channels/YOUR_USER_NAME/chatroom
3. Add the `PusherKey` and `PusherCluster` settings provided to you in `Settings/Kick.json`. If you don't have these, you may need to sign up for a developer account or ask LauraRozier/Yewnyx to provide them.
4. Add the numbers behind `id` in your browser to the `Settings/Kick.json` file's `ChatroomId` setting.
5. On the second run, the application will automatically connect to Kick's event API and start receiving events.

## Config Template

```json
{
  "Kick": {
    "PusherKey": "your-pusher-key",
    "PusherCluster": "your-pusher-cluster",
    "ChatroomId": "your-chatroom-id"
  }
}
```
