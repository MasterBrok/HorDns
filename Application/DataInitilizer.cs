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
        IReadOnlyList<Models.DnsCardModel>? dnss;
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
            dnss = JsonSerializer.Deserialize<List<Models.DnsCardModel>>(json, options)?.OrderByDescending(a=>a.CreateAt).ToList();
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