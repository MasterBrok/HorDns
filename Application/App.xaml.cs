using Application.Models;
using Application.Services;
using Application.UserControls;
using Application.Windows;
using System.Collections.ObjectModel;
using System.Windows;

namespace Application
{
    public partial class App : System.Windows.Application
    {
        public static DnsService DnsService = new DnsService();
        public static DnsCard Connect;

        public static event EventHandler<bool> DnsChanged;
        public static event EventHandler ForceDisconnect;
        
        public static ObservableCollection<DnsCard> Dnss = new();
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

        }
    }

}
