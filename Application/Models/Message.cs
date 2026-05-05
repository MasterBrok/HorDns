namespace Application.Models;

public class Message
{
    public string Text { get; set; }

    public Message(string text)
    {
        Text = text;
    }
}