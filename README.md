<img src="SubLink.png" alt="SubLink Logo" width="200" height="200">

# SubLink

SubLink is an application that helps you create scriptable integrations between Twitch and VRChat through OSC. You can customize your `SubLink.cs` file (included with the build) to respond to Twitch events with OSC and more

## Special Thanks

A big thank you to [CatGirlEddie](https://www.twitch.tv/catgirleddie), my collaborator on the avatar side of SubLink, for her invaluable contributions and expertise to SubLink-compatible avatar creation.

## Featured Streamers

SubLink is used by the following notable streamers (and more):

- [Roflgator](https://www.twitch.tv/roflgator)
- [MurderCrumpet](https://www.twitch.tv/murdercrumpet)
- [Aeriy](https://www.twitch.tv/aeriy)
- [MikaMoonlight](https://www.twitch.tv/mikamoonlight)

## Status
SubLink is currently very stable, and has been for over a year. I'm hitting pause on my personal contributions due to time constraints, but I'm not going anywhere, and can provide updates to resolve stability issues, or offer feedback on any contributions to SubLink as a whole. I would welcome contributions from the community; additional collaborators are encouraged and appreciated.

CatGirlEddie remains actively involved with and the foremost expert on creating and integrating SubLink features and animations into avatars.

## Setup

1. On the first run, the application will create a `settings.json` file.
2. Add your `clientid` and `clientsecret` obtained from your Twitch Developer account (or provided to you by CatGirlEddie to the `settings.json` file).
3. On the second run, the application will automatically authorize through Twitch and save an access token and refresh token to `settings.json`.

```json
{
  "Clientid": "your-client-id",
  "Clientsecret": "your-client-secret"
}
```

## Adding Support to Avatars

To add support for SubLink integrations to your VRChat avatars, I recommend using VRChat's avatar parameter drivers to increment an avatar parameter. For instance, when gift subs or bits come in, OSC will set an avatar parameter such as `TwitchCommunityGift` or `TwitchCheer` to the number gifted or cheered.

You can then create an animator layer with a resting state that transitions to a state with a parameter driver using the respective avatar parameter (e.g., `ExplosionQueue`). This animator layer will increment an internal parameter accordingly and reset the (OSC-set) avatar parameter to zero, allowing for manual radial menu fallback triggers.

From there, you can enqueue animations as needed based on the secondary parameters incremented by the parameter driver.

## Support

If you encounter any issues or need assistance, please open an issue in the project repository.

## Contributing

Contributions are welcome! If you have a feature idea, bug fix, or improvement, feel free to create a pull request or open an issue.

## Roadmap
Please note that the following roadmap represents the original plans for SubLink before it was put into maintenance mode.

1. **Cross-avatar coordination**: Implement a server component to facilitate interactions between avatars. This feature is not open-sourced, though some progress has been made in its development.
2. **Plugin system**: Develop a plugin system to extend SubLink's functionality and dynamically load assemblies that extend `SubLink.cs` capabilities.
3. **Support for other games**: Expand SubLink's capabilities to include integrations with other games.


## License

SubLink is released under the [MIT License](https://opensource.org/licenses/MIT).