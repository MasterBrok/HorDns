using System.Text.Json.Serialization;

namespace Application.Models;

public class Settings
{
    [JsonPropertyName("Dns")]
    public List<Dns> Dns { get; set; } = new();
}