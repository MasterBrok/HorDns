using System.Text.Json.Serialization;

namespace Application.Models;

public class Settings
{
    [JsonPropertyName("Dns")]
    public List<DnsCardModel> Dns { get; set; } = new();
}