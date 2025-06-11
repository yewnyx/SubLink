using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes;

internal class EventTypeConverter : JsonConverter<InEventMsg.BaseEventType> {
	private static readonly Dictionary<string, Type> EventTypeMap = new() {
        { "CurrentSceneCollectionChanging", typeof(InEventMsg.CurrentSceneCollectionChanging) },
        { "CurrentSceneCollectionChanged", typeof(InEventMsg.CurrentSceneCollectionChanged) },
        { "SceneCollectionListChanged", typeof(InEventMsg.SceneCollectionListChanged) },
        { "CurrentProfileChanging", typeof(InEventMsg.CurrentProfileChanging) },
        { "CurrentProfileChanged", typeof(InEventMsg.CurrentProfileChanged) },
        { "ProfileListChanged", typeof(InEventMsg.ProfileListChanged) },
        { "SourceFilterListReindexed", typeof(InEventMsg.SourceFilterListReindexed) },
        { "SourceFilterCreated", typeof(InEventMsg.SourceFilterCreated) },
        { "SourceFilterRemoved", typeof(InEventMsg.SourceFilterRemoved) },
        { "SourceFilterNameChanged", typeof(InEventMsg.SourceFilterNameChanged) },
        { "SourceFilterSettingsChanged", typeof(InEventMsg.SourceFilterSettingsChanged) },
        { "SourceFilterEnableStateChanged", typeof(InEventMsg.SourceFilterEnableStateChanged) },
        { "ExitStarted", typeof(InEventMsg.ExitStarted) },
        { "InputCreated", typeof(InEventMsg.InputCreated) },
        { "InputRemoved", typeof(InEventMsg.InputRemoved) },
        { "InputNameChanged", typeof(InEventMsg.InputNameChanged) },
        { "InputSettingsChanged", typeof(InEventMsg.InputSettingsChanged) },
        { "InputActiveStateChanged", typeof(InEventMsg.InputActiveStateChanged) },
        { "InputShowStateChanged", typeof(InEventMsg.InputShowStateChanged) },
        { "InputMuteStateChanged", typeof(InEventMsg.InputMuteStateChanged) },
        { "InputVolumeChanged", typeof(InEventMsg.InputVolumeChanged) },
        { "InputAudioBalanceChanged", typeof(InEventMsg.InputAudioBalanceChanged) },
        { "InputAudioSyncOffsetChanged", typeof(InEventMsg.InputAudioSyncOffsetChanged) },
        { "InputAudioTracksChanged", typeof(InEventMsg.InputAudioTracksChanged) },
        { "InputAudioMonitorTypeChanged", typeof(InEventMsg.InputAudioMonitorTypeChanged) },
        { "InputVolumeMeters", typeof(InEventMsg.InputVolumeMeters) },
        { "MediaInputPlaybackStarted", typeof(InEventMsg.MediaInputPlaybackStarted) },
        { "MediaInputPlaybackEnded", typeof(InEventMsg.MediaInputPlaybackEnded) },
        { "MediaInputActionTriggered", typeof(InEventMsg.MediaInputActionTriggered) },
        { "StreamStateChanged", typeof(InEventMsg.StreamStateChanged) },
        { "RecordStateChanged", typeof(InEventMsg.RecordStateChanged) },
        { "RecordFileChanged", typeof(InEventMsg.RecordFileChanged) },
        { "ReplayBufferStateChanged", typeof(InEventMsg.ReplayBufferStateChanged) },
        { "VirtualcamStateChanged", typeof(InEventMsg.VirtualcamStateChanged) },
        { "ReplayBufferSaved", typeof(InEventMsg.ReplayBufferSaved) },
        { "SceneItemCreated", typeof(InEventMsg.SceneItemCreated) },
        { "SceneItemRemoved", typeof(InEventMsg.SceneItemRemoved) },
        { "SceneItemListReindexed", typeof(InEventMsg.SceneItemListReindexed) },
        { "SceneItemEnableStateChanged", typeof(InEventMsg.SceneItemEnableStateChanged) },
        { "SceneItemLockStateChanged", typeof(InEventMsg.SceneItemLockStateChanged) },
        { "SceneItemSelected", typeof(InEventMsg.SceneItemSelected) },
        { "SceneItemTransformChanged", typeof(InEventMsg.SceneItemTransformChanged) },
        { "SceneCreated", typeof(InEventMsg.SceneCreated) },
        { "SceneRemoved", typeof(InEventMsg.SceneRemoved) },
        { "SceneNameChanged", typeof(InEventMsg.SceneNameChanged) },
        { "CurrentProgramSceneChanged", typeof(InEventMsg.CurrentProgramSceneChanged) },
        { "CurrentPreviewSceneChanged", typeof(InEventMsg.CurrentPreviewSceneChanged) },
        { "SceneListChanged", typeof(InEventMsg.SceneListChanged) },
        { "CurrentSceneTransitionChanged", typeof(InEventMsg.CurrentSceneTransitionChanged) },
        { "CurrentSceneTransitionDurationChanged", typeof(InEventMsg.CurrentSceneTransitionDurationChanged) },
        { "SceneTransitionStarted", typeof(InEventMsg.SceneTransitionStarted) },
        { "SceneTransitionEnded", typeof(InEventMsg.SceneTransitionEnded) },
        { "SceneTransitionVideoEnded", typeof(InEventMsg.SceneTransitionVideoEnded) },
        { "StudioModeStateChanged", typeof(InEventMsg.StudioModeStateChanged) },
        { "ScreenshotSaved", typeof(InEventMsg.ScreenshotSaved) },
        { "VendorEvent", typeof(InEventMsg.VendorEvent) },
        { "CustomEvent", typeof(InEventMsg.CustomEvent) }
	};

    // Any child type of ApiFieldType can be deserialized
    public override bool CanConvert(Type objectType) => typeof(InEventMsg.BaseEventType).IsAssignableFrom(objectType);

    // We'll get to this one in a bit...
    public override InEventMsg.BaseEventType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
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
                return JsonSerializer.Deserialize(ref readerAtStart, targetType, options) as InEventMsg.BaseEventType;

            throw new NotSupportedException($"{eventType} can not be deserialized");
        }

        throw new NotSupportedException("<unknown> can not be deserialized");
    }

    public override void Write(Utf8JsonWriter writer, InEventMsg.BaseEventType value, JsonSerializerOptions options)
    {
        // No need for this one in our use case, but to just dump the object into JSON
        // (without having the className property!), we can do this:
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
