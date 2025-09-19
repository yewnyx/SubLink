﻿using System;
using System.ComponentModel.DataAnnotations;

namespace OpenShock.SDK.CSharp.Models;

// ReSharper disable once ClassNeverInstantiated.Global
public class Control {
    public required Guid Id { get; set; }
    [EnumDataType(typeof(ControlType))]
    public required ControlType Type { get; set; }
    [Range(0, 100)]
    public required byte Intensity { get; set; }
    [Range(300, 30000)]
    public required ushort Duration { get; set; }
    public bool Exclusive { get; set; } = false;
}