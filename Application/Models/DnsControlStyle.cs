namespace Application.Models;

public class DnsControlStyle
{
    private string character;
    private string symbol;

    public int Id { get; set; }
    public string BorderColor { get; set; }
    public string Character
    {
        get
        {
            if (string.IsNullOrEmpty(character))
                return string.Empty;
            return new Uri($"pack://application:,,,/Resources/Assets/chibi/{character}").AbsolutePath;
        }
        set => character = value;
    }
    public string Symbol
    {
        get
        {
            if (string.IsNullOrEmpty(symbol))
                return string.Empty;
            return new Uri($"pack://application:,,,/Resources/Assets/chibi/{symbol}").AbsolutePath;
        }
        set => symbol = value;
    }
    public List<ColorItem> Colors { get; set; } = new();


    public static DnsControlStyle Default
    {
        get
        {
            return new DnsControlStyle()
            {
                Id = -1,
                BorderColor = "#e5edfa",
                Character = "1.png",
                Symbol = "heart1.png",
                Colors = [
                    new(){
                        Color = "#BD6D78",
                        Offset = 0.523
                    },
                    new(){
                        Color = "#C28A91",
                        Offset = 0.893
                    },
                    new(){
                        Color = "#BA6E78",
                        Offset = 0.744
                    }
                   ]
            };
        }
    }

}
