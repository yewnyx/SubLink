using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace tech.sublink.KickExtension.Kick.Types;

[TypeConverter(typeof(KickBadgeConverter))]
public sealed class KickBadge {
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("count"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public uint Count { get; set; } = 0;
}

public class KickBadgeConverter : TypeConverter {
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
        var casted = value as string;

        if (casted != null) {
            byte[] data = Convert.FromBase64String(casted);
            return JsonSerializer.Deserialize<KickBadge>(System.Text.Encoding.UTF8.GetString(data));
        }

        return base.ConvertFrom(context, culture, value);
    }

    public override object? ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
        var casted = value as KickBadge;

        if (destinationType == typeof(string) && casted != null) {
            var json = JsonSerializer.Serialize(casted);
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}

[TypeConverter(typeof(KickIdentityConverter))]
public sealed class KickIdentity {
    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;

    [JsonPropertyName("badges")]
    public KickBadge[] Badges { get; set; } = Array.Empty<KickBadge>();
}

public class KickIdentityConverter : TypeConverter {
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
        var casted = value as string;

        if (casted != null) {
            byte[] data = Convert.FromBase64String(casted);
            return JsonSerializer.Deserialize<KickIdentity>(System.Text.Encoding.UTF8.GetString(data));
        }

        return base.ConvertFrom(context, culture, value);
    }

    public override object? ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
        var casted = value as KickIdentity;

        if (destinationType == typeof(string) && casted != null) {
            var json = JsonSerializer.Serialize(casted);
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}

[TypeConverter(typeof(KickUserConverter))]
public sealed class KickUser {
    [JsonPropertyName("id"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public uint Id { get; set; } = 0;

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("identity")]
    public KickIdentity Identity { get; set; } = new();
}

public class KickUserConverter : TypeConverter {
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
        var casted = value as string;

        if (casted != null) {
            byte[] data = Convert.FromBase64String(casted);
            return JsonSerializer.Deserialize<KickUser>(System.Text.Encoding.UTF8.GetString(data));
        }

        return base.ConvertFrom(context, culture, value);
    }

    public override object? ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
        var casted = value as KickUser;

        if (destinationType == typeof(string) && casted != null) {
            var json = JsonSerializer.Serialize(casted);
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}

[TypeConverter(typeof(KickUserShortConverter))]
public sealed class KickUserShort {
    [JsonPropertyName("id"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public uint Id { get; set; } = 0;

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;
}

public class KickUserShortConverter : TypeConverter {
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
        var casted = value as string;

        if (casted != null) {
            byte[] data = Convert.FromBase64String(casted);
            return JsonSerializer.Deserialize<KickUserShort>(System.Text.Encoding.UTF8.GetString(data));
        }

        return base.ConvertFrom(context, culture, value);
    }

    public override object? ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
        var casted = value as KickUserShort;

        if (destinationType == typeof(string) && casted != null) {
            var json = JsonSerializer.Serialize(casted);
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}

[TypeConverter(typeof(PollOptionInfoConverter))]
public sealed class PollOptionInfo {
    [JsonPropertyName("id"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public uint Id { get; set; } = 0;

    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;

    [JsonPropertyName("votes"), JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public uint Votes { get; set; } = 0;
}

public class PollOptionInfoConverter : TypeConverter {
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
        var casted = value as string;

        if (casted != null) {
            byte[] data = Convert.FromBase64String(casted);
            return JsonSerializer.Deserialize<PollOptionInfo>(System.Text.Encoding.UTF8.GetString(data));
        }

        return base.ConvertFrom(context, culture, value);
    }

    public override object? ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
        var casted = value as PollOptionInfo;

        if (destinationType == typeof(string) && casted != null) {
            var json = JsonSerializer.Serialize(casted);
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
