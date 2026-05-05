using Application.Models;
using Application.UserControls;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace Application;

internal class DataInitilizer
{
    public static async Task InitialDns()
    {
        try
        {
            List<Dns> ips = new List<Dns>();
            await foreach (var line in File.ReadLinesAsync("dns.txt"))
            {
                try
                {
                    var splits = line.Split('=');
                    var sliptIp = splits[1].Split(',');
                    var ip = new Ip()
                    {
                        Preferred = sliptIp[0],
                        Alternate = sliptIp[1],
                    };
                    var dns = new Dns()
                    {
                        Ip = ip,
                        Title = splits[0]
                    };
                    ips.Add(dns);
                }
                catch (Exception)
                {

                }
            }
            Clipboard.SetText(JsonSerializer.Serialize(ips));
        }

        catch (Exception)
        {

        }
    }
    public static async IAsyncEnumerable<DnsCard> Initilizer()
    {
        IReadOnlyList<Dns>? dnss;
        IReadOnlyList<DnsControlStyle>? styles;
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var streamStyles = App.GetResourceStream(new("pack://application:,,,/styles.json")).Stream;
            styles = JsonSerializer.Deserialize<IReadOnlyList<DnsControlStyle>>(streamStyles);

            string json = await File.ReadAllTextAsync("setting.json");
            dnss = JsonSerializer.Deserialize<IReadOnlyList<Dns>>(json, options);

        }
        catch (Exception)
        {
            throw;
        }
        if (dnss is null)
        {
            throw new Exception();
        }
        int id = 0;
        Random rand = new Random(); 
        foreach (var dns in dnss)
        {
            dns.StyleId = rand.Next(1, styles.Count);
            DnsControlStyle style = styles?.FirstOrDefault(d => d.Id == dns.StyleId) ?? DnsControlStyle.Default;
            
            if (string.IsNullOrWhiteSpace(style?.Character))
                style.Character = $"{rand.Next(1, 6)}.png";

            if (string.IsNullOrWhiteSpace(style?.Symbol))
                style.Symbol = $"heart{rand.Next(1, 3)}.png";

            yield return new DnsCard
            {
                Resource = style,
                Dns = dns,
                Id = ++id
            };
        }
    }
}