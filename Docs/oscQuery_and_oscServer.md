## oscQuery

You can use this to advertise the parameters you want to receive from VRChat and other OSC applications. The syntax is rather simple, but does require a little explaination.  
For simple data types you can use the following syntax:

```csharp
oscQuery.AddEndpoint<bool/int/float>(
    string Parameter_Address,
    Attributes.AccessValues Parameter_Access,
    [object[] Parameter_Default_Value],
    [string Parameter_Description]
);

// Examples:
oscQuery.AddEndpoint<bool>("/avatar/parameters/MuteSelf", Attributes.AccessValues.ReadWrite, new object[] { true });
oscQuery.AddEndpoint<int>("/avatar/parameters/Viseme", Attributes.AccessValues.WriteOnly, new object[] { 0 }, "The viseme ID that's currently active");
oscQuery.AddEndpoint<float>("/avatar/parameters/ScaleFactor", Attributes.AccessValues.WriteOnly, description: "The scaling factor applied to the avatar");
```

For more advanced data types, like OSC trackers, you have to use the following syntax:

```csharp
oscQuery.AddEndpoint(
    string Parameter_Address,
    string Parameter_Type,
    Attributes.AccessValues Parameter_Access,
    [object[] Parameter_Default_Value],
    [string Parameter_Description]
);

// Examples:
oscQuery.AddEndpoint("/tracking/trackers/head/position", "fff", Attributes.AccessValues.ReadWrite, new object[] { 0.0f, 0.0f, 0.0f }, "The head position");
oscQuery.AddEndpoint("/tracking/trackers/head/rotation", "fff", Attributes.AccessValues.WriteOnly, description: "The head rotation");
oscQuery.AddEndpoint("/tracking/vrsystem/head/pose", "ffffff", Attributes.AccessValues.WriteOnly);
```

## oscServer

You can use this to receive data from VRChat and other OSC applications. The syntaxt is simple and relies on a message handling lambda.

```csharp
oscServer.TryAddMethod(
    string Parameter_Address,
    Action<OscMessageValues> Parameter_Values
);

// Examples:
oscServer.TryAddMethod("/avatar/parameters/MuteSelf", message => {
    _logger.Information($"VRChat mic mute changed to : {message.ReadBooleanElement(0)}");
});
oscServer.TryAddMethod("/avatar/parameters/Viseme", message => {
    _logger.Information($"VRChat viseme changed to : {message.ReadIntElement(0)}");
});
oscServer.TryAddMethod("/avatar/parameters/ScaleFactor", message => {
    _logger.Information($"VRChat avatar scaling changed to : {message.ReadFloatElement(0)}");
});
oscServer.TryAddMethod("/tracking/vrsystem/head/pose", message => {
    // To make this work add the following to the top of your sublink.cs file:
    // using BuildSoft.OscCore.UnityObjects;
    var position = new Vector3(message.ReadFloatElement(0), message.ReadFloatElement(1), message.ReadFloatElement(2));
    var rotation = new Vector3(message.ReadFloatElement(3), message.ReadFloatElement(4), message.ReadFloatElement(5));
    _logger.Information($"VRChat head tracking changed to : Pos{position} rot{rotation}");
});
```
