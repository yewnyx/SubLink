# SubLink DataTypes Discord Types

[Back To Readme](../../../README.md)  
[Back To Discord DataTypes Index](Index.md)

## DiscordErrorArgs

- `int`    Code    - Error code
- `string` Message - Error message

## DiscordVoiceSettingsEventArgs

- `float`  InputVolume       - Current voice input volume
- `float`  OutputVolume      - Current voice output volume
- `string` ModeType          - Voice mode type (VOICE_ACTIVITY or PUSH_TO_TALK)
- `bool`   ModeAutoThreshold - Indicates if the Voice Activity input threshold is automated or manual
- `float`  ModeThreshold     - Voice Activity input threshold
- `float`  ModeDelay         - Push-to-talk release delay
- `bool`   AutoGainControl   - Indicates if Automatic voice gain is active
- `bool`   EchoCancelation   - Indicates if Echo cancelation is active
- `bool`   Qos               - Indicates if Voice Qos is active
- `bool`   SilenceWarning    - Indicates if Discord will warn you when it's not detecting mic audio
- `bool`   Deaf              - Indicates if you are deafened
- `bool`   Mute              - Indicates if you ate muted

## DiscordVoiceStatusEventArgs

- `string` State     - Textual representation of the voice state
- `int`    StateCode - Numerical representation of the voice state

## DiscordChannelEventArgs

- `string` Id   - Discord channel ID
- `string` Name - Discord channel name
