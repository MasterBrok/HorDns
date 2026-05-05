using System.Text.Json.Serialization;

namespace Application.Models;

public class Dns
{
    public string Title { get; set; } = string.Empty;
    public Ip Ip { get; set; } = new();

    [JsonIgnore]
    public int StyleId { get; set; }
}
