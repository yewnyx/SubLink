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
