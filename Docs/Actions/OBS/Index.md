# SubLink Actions OBS

[Back To Readme](../../../README.md)

## SetSourceFilterEnabled

Sets the enable state of a source filter.

- Asynchronous
- Parameters
   - `string` sourceName - required - Name of the source the filter is on
   - `string` filterName - required - Name of the filter
   - `bool`   enabled    - required - New enable state of the filter; `true` to enable, `false` to disable
- Returns: Nothing

```csharp
await obs.SetSourceFilterEnabled("My Source", "My Filter", true);
await obs.SetSourceFilterEnabled("My Source", "My Filter 2", false);
```

## GetHotkeyList

Gets an array of all hotkey names in OBS.

Note: Hotkey functionality in obs-websocket comes as-is, and we do not guarantee support if things are broken. In 9/10 usages of hotkey requests, there exists a better, more reliable method via other requests.

- Asynchronous
- Parameters: Nothing
- Returns: `Array<String>`

```csharp
var hotkeys = await obs.GetHotkeyList();
```

## TriggerHotkeyByName

Triggers a hotkey using its name. See `GetHotkeyList`.

Note: Hotkey functionality in obs-websocket comes as-is, and we do not guarantee support if things are broken. In 9/10 usages of hotkey requests, there exists a better, more reliable method via other requests.

- Asynchronous
- Parameters
   - `string` hotkeyName  - required - Name of the hotkey
   - `string` contextName - optional - Name of the hotkey's context
- Returns: Nothing

```csharp
await obs.TriggerHotkeyByName("OBSBasic.StartStreaming");
await obs.TriggerHotkeyByName("OBSBasic.SelectScene", "Scene 2");
```

## TriggerHotkeyByKeySequence

Triggers a hotkey using a sequence of keys.

Note: Hotkey functionality in obs-websocket comes as-is, and we do not guarantee support if things are broken. In 9/10 usages of hotkey requests, there exists a better, more reliable method via other requests.

- Asynchronous
- Parameters
   - `string` keyId   - optional - The OBS key ID to use. See https://github.com/obsproject/obs-studio/blob/master/libobs/obs-hotkeys.h
   - `bool`   shift   - optional - Shift key state
   - `bool`   control - optional - Control key state
   - `bool`   alt     - optional - Alt key state
   - `bool`   command - optional - Command key state
- Returns: Nothing

```csharp
await obs.TriggerHotkeyByKeySequence("OBS_KEY_F3"); // F3
await obs.TriggerHotkeyByKeySequence("OBS_KEY_F3", true); // Shift + F3
await obs.TriggerHotkeyByKeySequence("OBS_KEY_F3", control: true); // Ctrl + F3
await obs.TriggerHotkeyByKeySequence(shift: true, control: true, alt: true); // Shift + Ctrl + Alt
```

## GetInputMute

Gets the audio mute state of an input.

Note: Special names
- desktop1 - Name of the Desktop Audio input
- desktop2 - Name of the Desktop Audio 2 input
- mic1 - Name of the Mic/Auxiliary Audio input
- mic2 - Name of the Mic/Auxiliary Audio 2 input
- mic3 - Name of the Mic/Auxiliary Audio 3 input
- mic4 - Name of the Mic/Auxiliary Audio 4 input

- Asynchronous
- Parameters
   - `string` name - required - Name of input to get the mute state of
- Returns: `bool`

```csharp
var isMuted = await obs.GetInputMute("desktop1");
var isMuted = await obs.GetInputMute("Game Capture");
var isMuted = await obs.GetInputMute("Browser");
```

## SetInputMute

Sets the audio mute state of an input.

Note: Special names
- desktop1 - Name of the Desktop Audio input
- desktop2 - Name of the Desktop Audio 2 input
- mic1 - Name of the Mic/Auxiliary Audio input
- mic2 - Name of the Mic/Auxiliary Audio 2 input
- mic3 - Name of the Mic/Auxiliary Audio 3 input
- mic4 - Name of the Mic/Auxiliary Audio 4 input

- Asynchronous
- Parameters
   - `string` name  - required - Name of the input to set the mute state of
   - `bool`   muted - required - Whether to mute the input or not; `true` to mute, `false` to unmute
- Returns: Nothing

```csharp
await obs.SetInputMute("desktop1", true);
await obs.SetInputMute("Game Capture", false);
await obs.SetInputMute("Browser", false);
```

## ToggleInputMute

Toggles the audio mute state of an input.

Note: Special names
- desktop1 - Name of the Desktop Audio input
- desktop2 - Name of the Desktop Audio 2 input
- mic1 - Name of the Mic/Auxiliary Audio input
- mic2 - Name of the Mic/Auxiliary Audio 2 input
- mic3 - Name of the Mic/Auxiliary Audio 3 input
- mic4 - Name of the Mic/Auxiliary Audio 4 input

- Asynchronous
- Parameters
   - `string` name - required - Name of the input to toggle the mute state of
- Returns: `bool`

```csharp
var isMuted = await obs.ToggleInputMute("desktop1");
var isMuted = await obs.ToggleInputMute("Game Capture");
var isMuted = await obs.ToggleInputMute("Browser");
```

## GetInputVolume

Gets the current volume setting of an input.

Note: Special names
- desktop1 - Name of the Desktop Audio input
- desktop2 - Name of the Desktop Audio 2 input
- mic1 - Name of the Mic/Auxiliary Audio input
- mic2 - Name of the Mic/Auxiliary Audio 2 input
- mic3 - Name of the Mic/Auxiliary Audio 3 input
- mic4 - Name of the Mic/Auxiliary Audio 4 input

- Asynchronous
- Parameters
   - `string` name - required - Name of the input to get the volume of
- Returns: `InputVolume`

```csharp
var volume = await obs.GetInputVolume("desktop1");
var volume = await obs.GetInputVolume("Game Capture");
var volume = await obs.GetInputVolume("Browser");
```

## SetInputVolume

Sets the volume setting of an input.

Note: Special names
- desktop1 - Name of the Desktop Audio input
- desktop2 - Name of the Desktop Audio 2 input
- mic1 - Name of the Mic/Auxiliary Audio input
- mic2 - Name of the Mic/Auxiliary Audio 2 input
- mic3 - Name of the Mic/Auxiliary Audio 3 input
- mic4 - Name of the Mic/Auxiliary Audio 4 input

- Asynchronous
- Parameters
   - `string` name       - required - Name of the input to set the volume of
   - `float`  multiplier - optional - Volume setting as multiplier; >= 0, <= 20
   - `float`  db         - optional - Volume setting in dB; >= -100, <= 26
- Returns: Nothing

```csharp
await obs.SetInputVolume("desktop1", 0.85); // Set via multiplier
await obs.SetInputVolume("Game Capture", db: -3); // Set via dB
await obs.SetInputVolume("Browser", 1, 0); // Set via both
```

## GetInputAudioSyncOffset

Gets the audio sync offset of an input.

Note: The audio sync offset can be negative too!

- Asynchronous
- Parameters
   - `string` name - required - Name of the input to get the audio sync offset of
- Returns: `int`

```csharp
var ofset = await obs.GetInputAudioSyncOffset("desktop1");
var ofset = await obs.GetInputAudioSyncOffset("Game Capture");
var ofset = await obs.GetInputAudioSyncOffset("Browser");
```

## SetInputAudioSyncOffset

Sets the audio sync offset of an input.

Note: The audio sync offset can be negative too!

- Asynchronous
- Parameters
   - `string` name       - required - Name of the input to set the audio sync offset of
   - `ofset`  offsetMs   - required - New audio sync offset in milliseconds; >= -950, <= 20000
- Returns: Nothing

```csharp
await obs.SetInputAudioSyncOffset("desktop1", 2);
await obs.SetInputAudioSyncOffset("Game Capture", db: 0);
await obs.SetInputAudioSyncOffset("Browser", -2);
```

## GetSceneItemEnabled

Gets the enable state of a scene item.
Works for Scenes and Groups

- Asynchronous
- Parameters
   - `string` name - required - Name of the scene the item is in
   - `uint`   id   - required - Numeric ID of the scene item; >= 0
- Returns: `bool`

```csharp
var enabled = await obs.GetSceneItemEnabled("desktop1");
```

## SetSceneItemEnabled

Sets the enable state of a scene item.
Works for Scenes and Groups

- Asynchronous
- Parameters
   - `string` name    - required - Name of the scene the item is in
   - `uint`   id      - required - Numeric ID of the scene item; >= 0
   - `bool`   enabled - required - New enable state of the scene item; `true` to enable, `false` to disable
- Returns: Nothing

```csharp
await obs.SetSceneItemEnabled("My Scene", 1, true);
await obs.SetSceneItemEnabled("My Scene", 4, false);
```

## GetActiveScene

Gets the current active scene.

- Asynchronous
- Parameters: Nothing
- Returns: `string`

```csharp
var sceneName = await obs.GetActiveScene();
```

## SetActiveScene

Sets the current active scene.

- Asynchronous
- Parameters
   - `string` sceneName      - required - Scene name to set as active for the outputs
   - `string` transitionName - required - Name of the transition to use; Defaults to `"Cut"`
- Returns: Nothing

```csharp
await obs.SetActiveScene("My Scene");
await obs.SetActiveScene("My Scene 2", "Super Cool Swipe");
```

## GetPreviewScene

Gets the current preview scene.
Only available when studio mode is enabled.

- Asynchronous
- Parameters: Nothing
- Returns: `string`

```csharp
var sceneName = await obs.GetPreviewScene();
```

## SetPreviewScene

Sets the current preview scene.
Only available when studio mode is enabled.

- Asynchronous
- Parameters
   - `string` name - required - Scene name to set as the current preview scene
- Returns: Nothing

```csharp
await obs.SetPreviewScene("My Scene");
await obs.SetPreviewScene("My Scene 2");
```

## GetCurrentSceneTransition

Gets information about the current scene transition.

- Asynchronous
- Parameters: Nothing
- Returns: `TransitionInfo`

```csharp
var transInfo = await obs.GetCurrentSceneTransition();
```

## SetCurrentSceneTransition

Sets the current scene transition.

Small note: While the namespace of scene transitions is generally unique, that uniqueness is not a guarantee as it is with other resources like inputs.

- Asynchronous
- Parameters
   - `string` name - required - Name of the transition to make active
- Returns: Nothing

```csharp
await obs.SetPreviewScene("Cut");
await obs.SetPreviewScene("My Super Epic Fade");
```

## TriggerTransition

Triggers the current scene transition. Same functionality as the `Transition` button in studio mode.
Only available when studio mode is enabled.

- Asynchronous
- Parameters: Nothing
- Returns: Nothing

```csharp
await obs.TriggerTransition();
```

## GetStudioModeEnabled

Gets whether studio is enabled.

- Asynchronous
- Parameters: Nothing
- Returns: `bool`

```csharp
var studioMode = await obs.GetStudioModeEnabled();
```
