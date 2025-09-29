using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes;

internal class ObsMediaInputActionConverter : JsonConverter<ObsMediaInputAction> {
    private static readonly Dictionary<ObsMediaInputAction, string> TranslationDict = new() {
        { ObsMediaInputAction.OBS_WEBSOCKET_MEDIA_INPUT_ACTION_NONE, "OBS_WEBSOCKET_MEDIA_INPUT_ACTION_NONE" },
        { ObsMediaInputAction.OBS_WEBSOCKET_MEDIA_INPUT_ACTION_PLAY, "OBS_WEBSOCKET_MEDIA_INPUT_ACTION_PLAY" },
        { ObsMediaInputAction.OBS_WEBSOCKET_MEDIA_INPUT_ACTION_PAUSE, "OBS_WEBSOCKET_MEDIA_INPUT_ACTION_PAUSE" },
        { ObsMediaInputAction.OBS_WEBSOCKET_MEDIA_INPUT_ACTION_STOP, "OBS_WEBSOCKET_MEDIA_INPUT_ACTION_STOP" },
        { ObsMediaInputAction.OBS_WEBSOCKET_MEDIA_INPUT_ACTION_RESTART, "OBS_WEBSOCKET_MEDIA_INPUT_ACTION_RESTART" },
        { ObsMediaInputAction.OBS_WEBSOCKET_MEDIA_INPUT_ACTION_NEXT, "OBS_WEBSOCKET_MEDIA_INPUT_ACTION_NEXT" },
        { ObsMediaInputAction.OBS_WEBSOCKET_MEDIA_INPUT_ACTION_PREVIOUS, "OBS_WEBSOCKET_MEDIA_INPUT_ACTION_PREVIOUS" }
    };

    public override ObsMediaInputAction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var jsonVal = reader.GetString();

        if (!string.IsNullOrWhiteSpace(jsonVal)) {
            foreach (var item in TranslationDict) {
                if (item.Value.Equals(jsonVal, StringComparison.InvariantCultureIgnoreCase))
                    return item.Key;
            }
        }

        return ObsMediaInputAction.OBS_WEBSOCKET_MEDIA_INPUT_ACTION_NONE;
    }

    public override void Write(Utf8JsonWriter writer, ObsMediaInputAction value, JsonSerializerOptions options) =>
        writer.WriteStringValue(
            TranslationDict.TryGetValue(value, out string? strVal)
                ? strVal
                : TranslationDict[ObsMediaInputAction.OBS_WEBSOCKET_MEDIA_INPUT_ACTION_NONE]
        );
}

internal class ObsOutputStateConverter : JsonConverter<ObsOutputState> {
    private static readonly Dictionary<ObsOutputState, string> TranslationDict = new() {
        { ObsOutputState.OBS_WEBSOCKET_OUTPUT_UNKNOWN, "OBS_WEBSOCKET_OUTPUT_UNKNOWN" },
        { ObsOutputState.OBS_WEBSOCKET_OUTPUT_STARTING, "OBS_WEBSOCKET_OUTPUT_STARTING" },
        { ObsOutputState.OBS_WEBSOCKET_OUTPUT_STARTED, "OBS_WEBSOCKET_OUTPUT_STARTED" },
        { ObsOutputState.OBS_WEBSOCKET_OUTPUT_STOPPING, "OBS_WEBSOCKET_OUTPUT_STOPPING" },
        { ObsOutputState.OBS_WEBSOCKET_OUTPUT_STOPPED, "OBS_WEBSOCKET_OUTPUT_STOPPED" },
        { ObsOutputState.OBS_WEBSOCKET_OUTPUT_RECONNECTING, "OBS_WEBSOCKET_OUTPUT_RECONNECTING" },
        { ObsOutputState.OBS_WEBSOCKET_OUTPUT_RECONNECTED, "OBS_WEBSOCKET_OUTPUT_RECONNECTED" },
        { ObsOutputState.OBS_WEBSOCKET_OUTPUT_PAUSED, "OBS_WEBSOCKET_OUTPUT_PAUSED" },
        { ObsOutputState.OBS_WEBSOCKET_OUTPUT_RESUMED, "OBS_WEBSOCKET_OUTPUT_RESUMED" }
    };

    public override ObsOutputState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var jsonVal = reader.GetString();

        if (!string.IsNullOrWhiteSpace(jsonVal)) {
            foreach (var item in TranslationDict) {
                if (item.Value.Equals(jsonVal, StringComparison.InvariantCultureIgnoreCase))
                    return item.Key;
            }
        }

        return ObsOutputState.OBS_WEBSOCKET_OUTPUT_UNKNOWN;
    }

    public override void Write(Utf8JsonWriter writer, ObsOutputState value, JsonSerializerOptions options) =>
        writer.WriteStringValue(
            TranslationDict.TryGetValue(value, out string? strVal)
                ? strVal
                : TranslationDict[ObsOutputState.OBS_WEBSOCKET_OUTPUT_UNKNOWN]
        );
}

internal class InputAudioMonitorTypeConverter : JsonConverter<InputAudioMonitorType> {
    private static readonly Dictionary<InputAudioMonitorType, string> TranslationDict = new() {
        { InputAudioMonitorType.OBS_MONITORING_TYPE_NONE, "OBS_MONITORING_TYPE_NONE" },
        { InputAudioMonitorType.OBS_MONITORING_TYPE_MONITOR_ONLY, "OBS_MONITORING_TYPE_MONITOR_ONLY" },
        { InputAudioMonitorType.OBS_MONITORING_TYPE_MONITOR_AND_OUTPUT, "OBS_MONITORING_TYPE_MONITOR_AND_OUTPUT" }
    };

    public override InputAudioMonitorType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var jsonVal = reader.GetString();

        if (!string.IsNullOrWhiteSpace(jsonVal)) {
            foreach (var item in TranslationDict) {
                if (item.Value.Equals(jsonVal, StringComparison.InvariantCultureIgnoreCase))
                    return item.Key;
            }
        }

        return InputAudioMonitorType.OBS_MONITORING_TYPE_NONE;
    }

    public override void Write(Utf8JsonWriter writer, InputAudioMonitorType value, JsonSerializerOptions options) =>
        writer.WriteStringValue(
            TranslationDict.TryGetValue(value, out string? strVal)
                ? strVal
                : TranslationDict[InputAudioMonitorType.OBS_MONITORING_TYPE_NONE]
        );
}
