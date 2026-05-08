using System.Text.Json.Serialization;

namespace Application.Models;

public class DnsCardModel
{
    public string Title { get; set; } = "Default";
    public Dns Dns { get; set; } = new();

    [JsonIgnore]
    public int StyleId { get; set; }

    public DateTime CreateAt { get; set; } = DateTime.Now;
}
