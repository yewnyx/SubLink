using System;

namespace OpenShock.SDK.CSharp.Models;

public class ResponseHub
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required DateTime CreatedOn { get; set; }
}