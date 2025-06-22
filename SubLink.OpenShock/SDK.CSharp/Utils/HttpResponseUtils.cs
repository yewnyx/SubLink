using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OpenShock.SDK.CSharp.Models;

namespace OpenShock.SDK.CSharp.Utils;

public static class HttpResponseUtils
{
    public static bool IsSuccess(this HttpResponseMessage responseMessage)
        => responseMessage.IsSuccessStatusCode &&
           responseMessage.Content.Headers.TryGetValues(
               "Content-Type", out var values) &&
           values.Any(x => x.StartsWith("application/json"));


    public static bool IsProblem(this HttpResponseMessage responseMessage) =>
        responseMessage.Content.Headers.TryGetValues("Content-Type", out var values) &&
        values.Any(x => x.StartsWith("application/problem+json"));

    public static async Task<T> ReadBaseResponseAsJsonAsync<T>(this HttpContent content,
        CancellationToken cancellationToken = default, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        var json = await ReadAsJsonAsync<BaseResponse<T>>(content, cancellationToken, jsonSerializerOptions);
        if (json.Data == null) throw new OpenShockSdkError("Response json data is null");
        return json.Data;
    }

    public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content,
        CancellationToken cancellationToken = default, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        jsonSerializerOptions ??= JsonSerializerOptions.Default;

#if NETSTANDARD2_1
        var jsonStream = await content.ReadAsStreamAsync();
#else
        var jsonStream = await content.ReadAsStreamAsync(cancellationToken);
#endif

        var json = await JsonSerializer.DeserializeAsync<T>(jsonStream, jsonSerializerOptions, cancellationToken);
        if (json == null) throw new OpenShockSdkError("Failed to deserialize response json");

        return json;
    }
}