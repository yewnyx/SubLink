# SubLink Actions OpenShock

[Back To Readme](../../../README.md)

## Legend

- [Vibrate Shocker](#VibrateShocker)
- [Sound Shocker](#SoundShocker)
- [Shock Shocker](#ShockShocker)
- [Stop Shocker](#StopShocker)
- [Get Own Shockers](#GetOwnShockers)
- [Pause Shocker](#PauseShocker)
- [Resume Shocker](#ResumeShocker)
- [Script Snippets](#Snippets)

## VibrateShocker

Make the shocker vibrate.

- Asynchronous
- Parameters
   - `string` shockerId - required - Shocker ID
   - `byte`   intensity - required - Vibration intensity; Minimum `0`, maximum `100`
   - `ushort` duration  - required - Vibration duration in ms; Minimum `300`, maximum `30000`
   - `bool`   exclusive - optional - Exclusivity indicator; `true` to be exclusive, `false` to be shared
- Returns: `bool` - `true` on success, `false` on failure

```csharp
var success = await openShock.VibrateShocker("123e4567-e89b-12d3-a456-426614174000", 50, 500);
var success = await openShock.VibrateShocker("123e4567-e89b-12d3-a456-426614174000", 100, 500, false);
```

[Back To Legend](#Legend)

## SoundShocker

Make the shocker sound an audio queue.

- Asynchronous
- Parameters
   - `string` shockerId - required - Shocker ID
   - `byte`   intensity - required - Sound intensity; Minimum `0`, maximum `100`
   - `ushort` duration  - required - Sound duration in ms; Minimum `300`, maximum `30000`
   - `bool`   exclusive - optional - Exclusivity indicator; `true` to be exclusive, `false` to be shared
- Returns: `bool` - `true` on success, `false` on failure

```csharp
var success = await openShock.SoundShocker("123e4567-e89b-12d3-a456-426614174000", 50, 500);
var success = await openShock.SoundShocker("123e4567-e89b-12d3-a456-426614174000", 100, 1000, false);
```

[Back To Legend](#Legend)

## ShockShocker

Make the shocker perform an actual shock.

- Asynchronous
- Parameters
   - `string` shockerId - required - Shocker ID
   - `byte`   intensity - required - Shock intensity; Minimum `0`, maximum `100`
   - `ushort` duration  - required - Shock duration in ms; Minimum `300`, maximum `30000`
   - `bool`   exclusive - optional - Exclusivity indicator; `true` to be exclusive, `false` to be shared
- Returns: `bool` - `true` on success, `false` on failure

```csharp
var success = await openShock.ShockShocker("123e4567-e89b-12d3-a456-426614174000", 50, 500);
var success = await openShock.ShockShocker("123e4567-e89b-12d3-a456-426614174000", 25, 300, false);
```

[Back To Legend](#Legend)

## StopShocker

Stop the shocker from performing any actions.

- Asynchronous
- Parameters
   - `string` shockerId - required - Shocker ID
- Returns: `bool` - `true` on success, `false` on failure

```csharp
var success = await openShock.StopShocker("123e4567-e89b-12d3-a456-426614174000");
```

[Back To Legend](#Legend)

## GetOwnShockers

Get all shockers belonging to the authenticated user.

- Asynchronous
- Parameters: Nothing
- Returns: `ResponseHubWithShockers`

```csharp
var shockerInfo = await openShock.GetOwnShockers();
```

[Back To Legend](#Legend)

## PauseShocker

Pause a shocker.

- Asynchronous
- Parameters
   - `string` shockerId - required - Shocker ID
- Returns: `bool` - `true` on success, `false` on failure

```csharp
var success = await openShock.PauseShocker("123e4567-e89b-12d3-a456-426614174000");
```

[Back To Legend](#Legend)

## ResumeShocker

Unpause a shocker.

- Asynchronous
- Parameters
   - `string` shockerId - required - Shocker ID
- Returns: `bool` - `true` on success, `false` on failure

```csharp
var success = await openShock.ResumeShocker("123e4567-e89b-12d3-a456-426614174000");
```

[Back To Legend](#Legend)

## Snippets

### Write the owned shockers to the console and log file

```csharp
async void LogOwnShockers() {
    var shockerInfo = await openShock.GetOwnShockers();
    string resultStr = "";

    foreach (var hub in shockerInfo) {
        resultStr += $"Hub `{hub.Name}` ({hub.Id}) has the following shockers:\r\n";

        foreach (var shocker in hub.Shockers) {
            resultStr += $"  - `{shocker.Name}` ({shocker.Id}) state: {(shocker.IsPaused ? "Paused" : "Live")}\r\n";
        }
    }

    logger.Information(resultStr);
}
```

[Back To Legend](#Legend)
