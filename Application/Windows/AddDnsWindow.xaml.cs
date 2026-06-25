using Application.Exceptions;
using Application.Models;
using Application.Services;
using System.ComponentModel;
using System.Windows;

namespace Application.Windows
{
    /// <summary>
    /// Interaction logic for AddDnsWindow.xaml
    /// </summary>
    public partial class AddDnsWindow : Window, INotifyPropertyChanged
    {
        public AddDnsWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Closing += AddDnsWindow_Closing;
            Dns = new();
        }

        private void AddDnsWindow_Closing(object? sender, CancelEventArgs e)
        {
            e.Cancel = !this.IsEnabled;
        }

        private DnsCardModel dns;

        public DnsCardModel Dns
        {
            get { return dns; }
            set
            {
                try
                {
                    dns = value;
                    PropertyChanged?.Invoke(this, new(nameof(Dns)));
                }
                catch (Exception)
                {

                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private async void AddDns_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                await Task.Delay(TimeSpan.FromSeconds(1));
                IpFormatException.ThrowIfNotValid(dns.Dns.Preferred);
                IpFormatException.ThrowIfNotValid(dns.Dns.Alternate);
                if (string.IsNullOrWhiteSpace(dns.Title))
                {
                    throw new ArgumentNullException(null, MessageText.TitleRequirment);
                }

                try
                {
                    await App.AddDns(dns);
                    Notification.Show(new("Add New Dns"), true);
                    Dns = new();
                }
                catch (Exception)
                {

                }

            }
            finally
            {
                this.IsEnabled = true;
                this.Close();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                DragMove();
        }
    }
}
