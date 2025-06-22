﻿using OpenShock.SDK.CSharp.Serialization;

namespace OpenShock.SDK.CSharp.Models;

[EnumAsInteger]
public enum ControlType
{
    Stop = 0,
    Shock = 1,
    Vibrate = 2,
    Sound = 3
}