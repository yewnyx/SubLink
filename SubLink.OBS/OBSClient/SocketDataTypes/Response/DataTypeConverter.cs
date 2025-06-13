using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace xyz.yewnyx.SubLink.OBS.OBSClient.SocketDataTypes.Response;

internal class DataTypeConverter : JsonConverter<InResponseMsg.Data> {
	private static readonly Dictionary<string, Type> ResponseTypeMap = new() {
        { "GetInputMute", typeof(GetInputMute) },
        { "ToggleInputMute", typeof(ToggleInputMute) },
        { "GetInputVolume", typeof(GetInputVolume) },
        { "GetInputAudioSyncOffset", typeof(GetInputAudioSyncOffset) },
        { "GetSceneItemEnabled", typeof(GetSceneItemEnabled) },
        { "GetCurrentProgramScene", typeof(GetCurrentProgramScene) },
        { "GetCurrentPreviewScene", typeof(GetCurrentPreviewScene) },
        { "GetCurrentSceneTransition", typeof(GetCurrentSceneTransition) },
        { "GetStudioModeEnabled", typeof(GetStudioModeEnabled) }
	};

    // Any child type of ApiFieldType can be deserialized
    public override bool CanConvert(Type objectType) => typeof(InResponseMsg.Data).IsAssignableFrom(objectType);

    // We'll get to this one in a bit...
    public override InResponseMsg.Data? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Check for null values
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        // Read the `className` from our JSON document
        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        var jsonObject = jsonDocument.RootElement;

        if (jsonObject.TryGetProperty("requestId", out var requestIdProp) &&
            jsonObject.TryGetProperty("requestType", out var requestTypeProp) &&
            jsonObject.TryGetProperty("requestStatus", out var requestStatusProp)) {
            string? requestType = requestTypeProp.GetString();

            // See if that class can be deserialized or not
            if (!string.IsNullOrWhiteSpace(requestType) &&
                requestStatusProp.TryGetProperty("result", out var resultProp) &&
                requestStatusProp.TryGetProperty("code", out var codeProp)) {
                InResponseMsg.Data result = new() {
                    RequestId = requestIdProp.GetString() ?? string.Empty,
                    RequestType = requestType,
                    RequestStatus = new() {
                        Result = resultProp.GetBoolean(),
                        Code = codeProp.GetInt32()
                    }
                };

                if (requestStatusProp.TryGetProperty("comment", out var commentProp))
                    result.RequestStatus.Comment = commentProp.GetString();

                if (ResponseTypeMap.TryGetValue(requestType, out var targetType) &&
                        jsonObject.TryGetProperty("responseData", out var responseDataProp))
                        result.ResponseData = responseDataProp.Deserialize(targetType, options) as IResponseType;

                return result;
            }

            throw new NotSupportedException($"{requestType} can not be deserialized");
        }

        throw new NotSupportedException("<unknown> can not be deserialized");
    }

    public override void Write(Utf8JsonWriter writer, InResponseMsg.Data value, JsonSerializerOptions options) =>
        // No need for this one in our use case, but to just dump the object into JSON
        // (without having the className property!), we can do this:
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
}
