using System;
using System.Text.Json.Serialization;
using OpenShock.SDK.CSharp.Serialization;

namespace OpenShock.SDK.CSharp.Models;

public class MinimalShocker
{
    public required Guid Id { get; set; }
    public required ushort RfId { get; set; }
    
    [JsonConverter(typeof(UnknownEnumTypeConverter<ShockerModelType>))]
    public required ShockerModelType Model { get; set; }
}

public class ShockerResponse : MinimalShocker
{
    public required string Name { get; set; }
    public required bool IsPaused { get; set; }
    public required DateTime CreatedOn { get; set; }
}