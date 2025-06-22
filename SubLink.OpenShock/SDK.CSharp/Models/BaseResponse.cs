
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Text.Json;

namespace OpenShock.SDK.CSharp.Models;

public class BaseResponse<T>
{
    public T? Data { get; init; }
    
    public override string ToString() => JsonSerializer.Serialize(this);
}