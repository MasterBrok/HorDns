using System.Text.Json.Serialization;

namespace Application.Models;

public class Ip
{
    public string Preferred { get; set; } = string.Empty;
    public string Alternate { get; set; } = string.Empty;
    
    public static Ip Epmty
    {
        get
        {
            return new();
        }
    }
    [JsonIgnore]
    public bool IsEpmty
    {
        get
        {
            return string.IsNullOrWhiteSpace(Preferred) && string.IsNullOrWhiteSpace(Alternate);
        }
    }
    public override bool Equals(object? obj)
    {
        if (obj is Ip ip)
        {
            return this.Preferred == ip.Preferred && this.Alternate == ip.Alternate;
        }
        return false;
    }
}
