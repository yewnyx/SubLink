using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes.Event;

internal class EventTypeConverter : JsonConverter<EventDataType> {
	private static readonly Dictionary<string, Type> EventTypeMap = new() {
        { "CurrentSceneCollectionChanging", typeof(CurrentSceneCollectionChanging) },
        { "CurrentSceneCollectionChanged", typeof(CurrentSceneCollectionChanged) },
        { "SceneCollectionListChanged", typeof(SceneCollectionListChanged) },
        { "CurrentProfileChanging", typeof(CurrentProfileChanging) },
        { "CurrentProfileChanged", typeof(CurrentProfileChanged) },
        { "ProfileListChanged", typeof(ProfileListChanged) },
        { "SourceFilterListReindexed", typeof(SourceFilterListReindexed) },
        { "SourceFilterCreated", typeof(SourceFilterCreated) },
        { "SourceFilterRemoved", typeof(SourceFilterRemoved) },
        { "SourceFilterNameChanged", typeof(SourceFilterNameChanged) },
        { "SourceFilterSettingsChanged", typeof(SourceFilterSettingsChanged) },
        { "SourceFilterEnableStateChanged", typeof(SourceFilterEnableStateChanged) },
        { "ExitStarted", typeof(ExitStarted) },
        { "InputCreated", typeof(InputCreated) },
        { "InputRemoved", typeof(InputRemoved) },
        { "InputNameChanged", typeof(InputNameChanged) },
        { "InputSettingsChanged", typeof(InputSettingsChanged) },
        { "InputActiveStateChanged", typeof(InputActiveStateChanged) },
        { "InputShowStateChanged", typeof(InputShowStateChanged) },
        { "InputMuteStateChanged", typeof(InputMuteStateChanged) },
        { "InputVolumeChanged", typeof(InputVolumeChanged) },
        { "InputAudioBalanceChanged", typeof(InputAudioBalanceChanged) },
        { "InputAudioSyncOffsetChanged", typeof(InputAudioSyncOffsetChanged) },
        { "InputAudioTracksChanged", typeof(InputAudioTracksChanged) },
        { "InputAudioMonitorTypeChanged", typeof(InputAudioMonitorTypeChanged) },
        { "InputVolumeMeters", typeof(InputVolumeMeters) },
        { "MediaInputPlaybackStarted", typeof(MediaInputPlaybackStarted) },
        { "MediaInputPlaybackEnded", typeof(MediaInputPlaybackEnded) },
        { "MediaInputActionTriggered", typeof(MediaInputActionTriggered) },
        { "StreamStateChanged", typeof(StreamStateChanged) },
        { "RecordStateChanged", typeof(RecordStateChanged) },
        { "RecordFileChanged", typeof(RecordFileChanged) },
        { "ReplayBufferStateChanged", typeof(ReplayBufferStateChanged) },
        { "VirtualcamStateChanged", typeof(VirtualcamStateChanged) },
        { "ReplayBufferSaved", typeof(ReplayBufferSaved) },
        { "SceneItemCreated", typeof(SceneItemCreated) },
        { "SceneItemRemoved", typeof(SceneItemRemoved) },
        { "SceneItemListReindexed", typeof(SceneItemListReindexed) },
        { "SceneItemEnableStateChanged", typeof(SceneItemEnableStateChanged) },
        { "SceneItemLockStateChanged", typeof(SceneItemLockStateChanged) },
        { "SceneItemSelected", typeof(SceneItemSelected) },
        { "SceneItemTransformChanged", typeof(SceneItemTransformChanged) },
        { "SceneCreated", typeof(SceneCreated) },
        { "SceneRemoved", typeof(SceneRemoved) },
        { "SceneNameChanged", typeof(SceneNameChanged) },
        { "CurrentProgramSceneChanged", typeof(CurrentProgramSceneChanged) },
        { "CurrentPreviewSceneChanged", typeof(CurrentPreviewSceneChanged) },
        { "SceneListChanged", typeof(SceneListChanged) },
        { "CurrentSceneTransitionChanged", typeof(CurrentSceneTransitionChanged) },
        { "CurrentSceneTransitionDurationChanged", typeof(CurrentSceneTransitionDurationChanged) },
        { "SceneTransitionStarted", typeof(SceneTransitionStarted) },
        { "SceneTransitionEnded", typeof(SceneTransitionEnded) },
        { "SceneTransitionVideoEnded", typeof(SceneTransitionVideoEnded) },
        { "StudioModeStateChanged", typeof(StudioModeStateChanged) },
        { "ScreenshotSaved", typeof(ScreenshotSaved) },
        { "VendorEvent", typeof(VendorEvent) },
        { "CustomEvent", typeof(CustomEvent) }
	};

    // Any child type of ApiFieldType can be deserialized
    public override bool CanConvert(Type objectType) => typeof(EventDataType).IsAssignableFrom(objectType);

    // We'll get to this one in a bit...
    public override EventDataType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        // Check for null values
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        // Copy the current state from reader (it's a struct)
        var readerAtStart = reader;

        // Read the `className` from our JSON document
        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        var jsonObject = jsonDocument.RootElement;

        if (jsonObject.TryGetProperty("eventType", out var eventTypeProp)) {
            string? eventType = eventTypeProp.GetString();

            // See if that class can be deserialized or not
            if (!string.IsNullOrWhiteSpace(eventType) && EventTypeMap.TryGetValue(eventType, out var targetType))
                // Deserialize it
                return JsonSerializer.Deserialize(ref readerAtStart, targetType, options) as EventDataType;

            throw new NotSupportedException($"{eventType} can not be deserialized");
        }

        throw new NotSupportedException("<unknown> can not be deserialized");
    }

    public override void Write(Utf8JsonWriter writer, EventDataType value, JsonSerializerOptions options) {
        // No need for this one in our use case, but to just dump the object into JSON
        // (without having the className property!), we can do this:
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
