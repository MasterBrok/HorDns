using Application.Models;
using Application.Windows;
using System.Windows.Media;

namespace Application.Services;

public class Notification
{
    public static void Show(Message message,bool isSuccess = false)
    {
        NotifiactionWindow notifiaction = new()
        {
            Owner = App.Current.MainWindow,
            Image = isSuccess ? App.Current.Resources["Success"] as ImageSource : App.Current.Resources["Error"] as ImageSource
        };
        notifiaction.Show();
        notifiaction.Open(message);
        notifiaction.Close();
    }

    public static void Hide()
    {
        NotifiactionWindow notifiaction = new()
        {
            Owner = App.Current.MainWindow,
            Image = App.Current.Resources["Success"] as ImageSource,
        };
        notifiaction.Show();
        notifiaction.Open(new(MessageText.Hide));
        notifiaction.Close();
    }
    public static void Bye()
    {
        NotifiactionWindow notifiaction = new()
        {
            Owner = App.Current.MainWindow,
            Image = App.Current.Resources["Kis"] as ImageSource
        };
        notifiaction.Show();
        notifiaction.Open(new(MessageText.Bye));
        notifiaction.Close();
    }
    public static void Connect()
    {
        NotifiactionWindow notifiaction = new()
        {
            Owner = App.Current.MainWindow,
            Image = App.Current.Resources["Connect"] as ImageSource
        };
        notifiaction.Show();
        notifiaction.Open(new(MessageText.Connected));
        notifiaction.Close();
    }
    public static void Disconnect()
    {
        NotifiactionWindow notifiaction = new()
        {
            Owner = App.Current.MainWindow,
            Image = App.Current.Resources["Disconnect"] as ImageSource
        };
        notifiaction.Show();
        notifiaction.Open(new(MessageText.Disconnected));
        notifiaction.Close();
    }
}
