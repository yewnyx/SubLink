# SubLink DataTypes OBS Types

[Back To Readme](../../../README.md)  
[Back To OBS DataTypes Index](Index.md)

## InputVolume

- `float` Multiplier - Volume setting as multiplier
- `float` DB         - Volume setting in dB

## TransitionInfo

- `string` Name           - Name of the transition
- `string` Kind           - Kind of the transition
- `bool`   IsFixed        - Whether the transition uses a fixed (unconfigurable) duration
- `uint?`  Duration       - Configured transition duration in milliseconds. `null` if transition is fixed
- `bool`   IsConfigurable - Whether the transition supports being configured

## CurrentSceneCollectionChanging

- `string` SceneCollectionName - Name of the current scene collection

## CurrentSceneCollectionChanged

- `string` SceneCollectionName - Name of the new scene collection

## SceneCollectionListChanged

- `string[]` SceneCollections - Updated list of scene collections

## CurrentProfileChanging

- `string` ProfileName - Name of the current profile

## CurrentProfileChanged

- `string` ProfileName - Name of the new profile

## ProfileListChanged

- `string[]` Profiles - Updated list of profiles

## SourceFilterListReindexed

- `string`       SourceName - Name of the source
- `JsonObject[]` Filters    - Array of filter objects

## SourceFilterCreated

- `string`     SourceName            - Name of the source the filter is on
- `string`     FilterName            - Name of the filter
- `string`     FilterKind            - Kind of the filter
- `uint`       FilterIndex           - Index position of the filter
- `JsonObject` FilterSettings        - Settings configured to the filter when it was created
- `JsonObject` DefaultFilterSettings - Default settings for the filter

## SourceFilterRemoved

- `string` SourceName - Name of the source the filter is on
- `string` FilterName - Name of the filter

## SourceFilterNameChanged

- `string` SourceName    - Name of the source the filter is on
- `string` OldFilterName - Old name of the filter
- `string` FilterName    - New name of the filter

## SourceFilterSettingsChanged

- `string`     SourceName     - Name of the source the filter is on
- `string`     FilterName     - Name of the filter
- `JsonObject` FilterSettings - New settings object of the filter

## SourceFilterEnableStateChanged

- `string` SourceName    - Name of the source the filter is on
- `string` FilterName    - Name of the filter
- `bool`   FilterEnabled - Whether the filter is enabled

## InputCreated

- `string`     InputName            - Name of the input
- `string`     InputUuid            - UUID of the input
- `string`     InputKind            - Kind of the input
- `string`     UnversionedInputKind - Unversioned kind of input (aka no `_v2` stuff)
- `JsonObject` InputSettings        - Settings configured to the input when it was created
- `JsonObject` DefaultInputSettings - Default settings for the input

## InputRemoved

- `string`     InputName     - Name of the input
- `string`     InputUuid     - UUID of the input

## InputNameChanged

- `string`     InputUuid    - UUID of the input
- `string`     OldInputName - Old name of the input
- `string`     InputName    - New name of the input

## InputSettingsChanged

- `string`     InputName     - Name of the input
- `string`     InputUuid     - UUID of the input
- `JsonObject` InputSettings - New settings object of the input

## InputActiveStateChanged

- `string` InputName   - Name of the input
- `string` InputUuid   - UUID of the input
- `bool`   VideoActive - Whether the input is active

## InputShowStateChanged

- `string` InputName    - Name of the input
- `string` InputUuid    - UUID of the input
- `bool`   VideoShowing - Whether the input is showing

## InputMuteStateChanged

- `string` InputName  - Name of the input
- `string` InputUuid  - UUID of the input
- `bool`   InputMuted - Whether the input is muted

## InputVolumeChanged

- `string` InputName      - Name of the input
- `string` InputUuid      - UUID of the input
- `float`  InputVolumeMul - New volume level multiplier
- `float` InputVolumeDb   - New volume level in dB

## InputAudioBalanceChanged

- `string` InputName         - Name of the input
- `string` InputUuid         - UUID of the input
- `float`  InputAudioBalance - New audio balance value of the input

## InputAudioSyncOffsetChanged

- `string` InputName            - Name of the input
- `string` InputUuid            - UUID of the input
- `int`    InputAudioSyncOffset - New sync offset in milliseconds

## InputAudioTracksChanged

- `string`     InputName        - Name of the input
- `string`     InputUuid        - UUID of the input
- `JsonObject` InputAudioTracks - Object of audio tracks along with their associated enable states

## InputAudioMonitorTypeChanged

- `string` InputName   - Name of the input
- `string` InputUuid   - UUID of the input
- `string` MonitorType - New monitor type of the input; Available types are: `OBS_MONITORING_TYPE_NONE`, `OBS_MONITORING_TYPE_MONITOR_ONLY`, `OBS_MONITORING_TYPE_MONITOR_AND_OUTPUT`

## InputVolumeMeters

- `JsonObject[]` Inputs - Array of active inputs with their associated volume levels

## MediaInputPlaybackStarted

- `string` InputName - Name of the input
- `string` InputUuid - UUID of the input

## MediaInputPlaybackEnded

- `string` InputName - Name of the input
- `string` InputUuid - UUID of the input

## MediaInputActionTriggered

- `string` InputName   - Name of the input
- `string` InputUuid   - UUID of the input
- `string` MediaAction - Action performed on the input. See `ObsMediaInputAction` enum

## StreamStateChanged

- `bool`    OutputActive - Whether the output is active
- `string`  OutputState  - Specific state of the output

## RecordStateChanged

- `bool`    OutputActive - Whether the output is active
- `string`  OutputState  - Specific state of the output
- `string?` OutputPath   - File name for the saved recording, if record stopped. `null` otherwise

## RecordFileChanged

- `string` NewOutputPath - File name that the output has begun writing to

## ReplayBufferStateChanged

- `bool`   OutputActive - Whether the output is active
- `string` OutputState  - Specific state of the output

## VirtualcamStateChanged

- `bool`   OutputActive - Whether the output is active
- `string` OutputState  - Specific state of the output

## ReplayBufferSaved

- `string` SavedReplayPath - Path of the saved replay file

## SceneItemCreated

- `string` SceneName      - Name of the scene the item was added to
- `string` SceneUuid      - UUID of the scene the item was added to
- `string` SourceName     - Name of the underlying source (input/scene)
- `string` SourceUuid     - UUID of the underlying source (input/scene)
- `uint`   SceneItemId    - Numeric ID of the scene item
- `uint`   SceneItemIndex - Index position of the item

## SceneItemRemoved

- `string` SceneName   - Name of the scene the item was removed from
- `string` SceneUuid   - UUID of the scene the item was removed from
- `string` SourceName  - Name of the underlying source (input/scene)
- `string` SourceUuid  - UUID of the underlying source (input/scene)
- `uint`   SceneItemId - Numeric ID of the scene item

## SceneItemListReindexed

- `string`       SceneName  - Name of the scene
- `string`       SceneUuid  - UUID of the scene
- `JsonObject[]` SceneItems - Array of scene item objects

## SceneItemEnableStateChanged

- `string` SceneName        - Name of the scene the item is in
- `string` SceneUuid        - UUID of the scene the item is in
- `uint`   SceneItemId      - Numeric ID of the scene item
- `bool`   SceneItemEnabled - Whether the scene item is enabled (visible)

## SceneItemLockStateChanged

- `string` SceneName       - Name of the scene the item is in
- `string` SceneUuid       - UUID of the scene the item is in
- `uint`   SceneItemId     - Numeric ID of the scene item
- `bool`   SceneItemLocked - Whether the scene item is locked

## SceneItemSelected

- `string` SceneName   - Name of the scene the item is in
- `string` SceneUuid   - UUID of the scene the item is in
- `uint`   SceneItemId - Numeric ID of the scene item

## SceneItemTransformChanged

- `string`     SceneName          - Name of the scene the item is in
- `string`     SceneUuid          - UUID of the scene the item is in
- `uint`       SceneItemId        - Numeric ID of the scene item
- `JsonObject` SceneItemTransform - New transform/crop info of the scene item

## SceneCreated

- `string` SceneName - Name of the new scene
- `string` SceneUuid - UUID of the new scene
- `bool`   IsGroup   - Whether the new scene is a group

## SceneRemoved

- `string` SceneName - Name of the removed scene
- `string` SceneUuid - UUID of the removed scene
- `bool`   IsGroup   - Whether the scene was a group

## SceneNameChanged

- `string` SceneUuid    - UUID of the scene
- `string` OldSceneName - Old name of the scene
- `string` SceneName    - New name of the scene

## CurrentProgramSceneChanged

- `string` SceneName - Name of the scene that was switched to
- `string` SceneUuid - UUID of the scene that was switched to

## CurrentPreviewSceneChanged

- `string` SceneName - Name of the scene that was switched to
- `string` SceneUuid - UUID of the scene that was switched to

## SceneListChanged

- `JsonObject[]` Scenes - Updated array of scenes

## CurrentSceneTransitionChanged

- `string` TransitionName - Name of the new transition
- `string` TransitionUuid - UUID of the new transition

## CurrentSceneTransitionDurationChanged

- `uint` TransitionDuration - Transition duration in milliseconds

## SceneTransitionStarted

- `string` TransitionName - Scene transition name
- `string` TransitionUuid - Scene transition UUID


## SceneTransitionEnded

- `string` TransitionName - Scene transition name
- `string` TransitionUuid - Scene transition UUID


## SceneTransitionVideoEnded

- `string` TransitionName - Scene transition name
- `string` TransitionUuid - Scene transition UUID

## StudioModeStateChanged

- `bool` StudioModeEnabled - Indicated whether Studio Mode is enabled; `true` when enabled, `false` when disabled

## ScreenshotSaved

- `string` SavedScreenshotPath - Path of the saved image file

## VendorEvent

- `string`     VendorName - Name of the vendor emitting the event
- `string`     EventType  - Vendor-provided event typedef
- `JsonObject` EventData  - Vendor-provided event data. Empty object if event does not provide any data

## CustomEvent

- `JsonObject` EventData - Custom Event Data
