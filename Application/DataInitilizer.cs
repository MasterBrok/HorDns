using Application.Models;
using Application.Services;
using Application.UserControls;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace Application;

internal class DataInitilizer
{
    public static async Task InitialDns()
    {
        try
        {
            List<Models.DnsCardModel> ips = new List<Models.DnsCardModel>();
            await foreach (var line in File.ReadLinesAsync("dns.txt"))
            {
                try
                {
                    var splits = line.Split('=');
                    var sliptIp = splits[1].Split(',');
                    var ip = new Dns()
                    {
                        Preferred = sliptIp[0],
                        Alternate = sliptIp[1],
                    };

                    var card = new Models.DnsCardModel()
                    {
                        Dns = ip,
                        Title = splits[0],
                    };
                    ips.Add(card);
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
    public static async IAsyncEnumerable<UserControls.DnsCard> Initilizer()
    {
        IReadOnlyList<DnsCardModel>? dnss = new List<DnsCardModel>();
        IReadOnlyList<DnsControlStyle>? styles = new List<DnsControlStyle>();
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var streamStyles = App.GetResourceStream(new("pack://application:,,,/styles.json")).Stream;
            styles = JsonSerializer.Deserialize<IReadOnlyList<DnsControlStyle>>(streamStyles);

#if DEBUG

            var dnsStream = App.GetResourceStream(new("pack://application:,,,/defsetting.json")).Stream;
            dnss = JsonSerializer.Deserialize<List<Models.DnsCardModel>>(dnsStream, options)?.OrderByDescending(a => a.CreateAt)
                 .ToList();
#else
            if (!File.Exists("setting.json"))
            {
                using var file = File.Create("setting.json");
            }
            string json = await File.ReadAllTextAsync("setting.json");
            if (!string.IsNullOrWhiteSpace(json))
            {
                dnss = JsonSerializer.Deserialize<List<Models.DnsCardModel>>(json.Trim(), options)?.OrderByDescending(a => a.CreateAt)
                 .ToList();
            }
#endif


        }
        catch (Exception e)
        {
            Notification.Show(new("Oops..."), false);
        }

        if (dnss is null)
        {
            Notification.Show(new("Dns list is empty"), false);
        }
        int id = 0;
        Random rand = new Random();
        foreach (var dns in dnss)
        {
            dns.StyleId = rand.Next(1, styles.Count);
            DnsControlStyle? cardStyle = styles?.FirstOrDefault(d => d.Id == dns.StyleId) ?? DnsControlStyle.Default;
            if (cardStyle is not null)
            {
                if (string.IsNullOrWhiteSpace(cardStyle.Character))
                    cardStyle.Character = $"{rand.Next(1, 6)}.png";

                if (string.IsNullOrWhiteSpace(cardStyle?.Symbol))
                    cardStyle.Symbol = $"heart{rand.Next(1, 3)}.png";

                yield return new DnsCard
                {
                    Resource = cardStyle,
                    Dns = dns,
                    Id = ++id
                };
            }
        }
    }

}