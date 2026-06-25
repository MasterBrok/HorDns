using Application.Models;
using Application.Services;
using Application.UserControls;
using Application.Windows;
using System.IO;
using System.Text.Json;


namespace Application
{
    public partial class App : System.Windows.Application
    {
        public static DnsService DnsService = new DnsService();
        public static DnsCard Connect;

        public static event EventHandler<bool> DnsChanged;
        public static event EventHandler ForceDisconnect;
        public static event EventHandler ListUpdated;

        public static bool IsUpdating = false;

        public static List<DnsCardModel> DnsList = new();
        public static void OnDnsChanged(object sender, bool result)
        {
            DnsChanged?.Invoke(sender, result);
        }
        public static void OnForceDisconnect(object sender)
        {
            ForceDisconnect?.Invoke(sender, EventArgs.Empty);
        }
        public static void Notify(Message message)
        {
            Notification.Show(message);
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            Notification.Show(new(e.Exception.Message), false);
        }

        public static async Task AddDns(DnsCardModel dns)
        {
            try
            {
                AddToList(dns);
                var json = JsonSerializer.Serialize<List<DnsCardModel>>(DnsList);
                await File.WriteAllTextAsync("setting.json", json);
                ListUpdated?.Invoke(new AddDnsWindow(), EventArgs.Empty);
            }
            catch (Exception)
            {

            }
        }
        public static void AddToList(DnsCardModel dns)
        {
            DnsList.Add(dns);
        }
    }

}
