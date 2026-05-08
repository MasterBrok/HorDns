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
            App.ListUpdated += App_ListUpdated;
        }

        private async void App_ListUpdated(object? sender, EventArgs e)
        {
            wpDns.Children.Clear();
            await Update();
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

        public async Task Update()
        {
            try
            {
                App.IsUpdating = true;
                var currentDns = App.DnsService.GetCurrentDns();

                await foreach (var card in DataInitilizer.Initilizer())
                {
                    await Task.Delay(300);
                    if (currentDns is not null && card.Dns.Dns.Equals(currentDns))
                        card.Connect();
                    App.AddToList(card.Dns);
                    wpDns.Children.Add(card);
                }

                this.wpDns.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                App.IsUpdating = false;
            }
        }
        private async void Dashboard_Loaded(object sender, RoutedEventArgs e)
        {
            await Update();
        }
    }
}
