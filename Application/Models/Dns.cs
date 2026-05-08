using Application.Conventers;
using Application.Services;
using System.Text.Json.Serialization;

namespace Application.Models;

public class Dns
{
    public Ip Preferred { get; set; } = Ip.Empty;
    public Ip Alternate { get; set; } = Ip.Empty;

    public static Dns Epmty
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
            return Alternate == Ip.Empty && Preferred == Ip.Empty;
        }
    }
    public override bool Equals(object? obj)
    {
        if (obj is Dns ip)
        {
            return this.Preferred == ip.Preferred && this.Alternate == ip.Alternate;
        }
        return false;
    }
}

[JsonConverter(typeof(IpJsonConventer))]
public class Ip
{
    public string Value { get; set; }
    public Ip(string value)
    {
        Value = value;
    }
    public static Ip Empty
    {
        get
        {
            return new("0.0.0.0");
        }
    }
    public override string ToString()
    {
        return Value;
    }
    public static bool operator == (Ip a, Ip b)
    {
        return string.Equals(a.Value, b.Value);
    }
    public static bool operator !=(Ip a, Ip b) => !(a == b);

    public static implicit operator Ip(string v) => new(v);
}