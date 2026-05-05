using Application.Services;
using Application.UserControls;
using System.Windows;
using System.Windows.Controls;

namespace Application.Pages
{
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
            Loaded += Dashboard_Loaded;
            App.ForceDisconnect += App_ForceDisconnect;
        }

        private void App_ForceDisconnect(object? sender, EventArgs e)
        {
            try
            {
                App.Connect?.Disconnect();
            }
            catch (Exception ex)
            {

            }
        }

        private async void Dashboard_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentDns = App.DnsService.GetCurrentDns();

                await foreach (var card in DataInitilizer.Initilizer())
                {
                    await Task.Delay(300);
                    if (currentDns is not null && card.Dns.Ip.Equals(currentDns))
                        card.Connect();
                    wpDns.Children.Add(card);
                }
                //wpDns.Children.OfType<DnsCard>().FirstOrDefault(a => a.Dns.Ip.Equals(currentDns))?.Connect();
                this.wpDns.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
