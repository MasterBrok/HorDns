using Application.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Conventers;

public class IpJsonConventer : JsonConverter<Ip>
{
    public override Ip? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value is null ? Ip.Empty : new Ip(value);
    }

    public override void Write(Utf8JsonWriter writer, Ip value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}
