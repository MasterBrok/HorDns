using Application.Models;
using Application.Pages;
using Application.Services;
using Application.Windows;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Application
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            App.DnsChanged += App_ResultSetDns;
            Closing += MainWindow_Closing;
        }




        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            e.Cancel = true;
            WindowState = WindowState.Minimized;
            ShowInTaskbar = false;
            Hide();
        }

        private void App_AccessDinied(object? sender, EventArgs e)
        {
            Notification.Show(new(MessageText.AccessDenied));
        }

        private void App_ResultSetDns(object? sender, bool e)
        {
            Notification.Show(new(e ? MessageText.Connected : MessageText.Failed));
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }




        private async void Exit_Click(object sender, RoutedEventArgs e)
        {
            //await Task.Delay(TimeSpan.FromSeconds(2));
            WindowState = WindowState.Minimized;
            ShowInTaskbar = false;
            Hide();
            Notification.Hide();
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.Content is not Dashboard)
                Frame.NavigationService.Navigate(new Dashboard());

        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            //if (Frame.Content is not Dashboard)
            //    Frame.NavigationService.Navigate(new Dashboard());
        }

        private void MyNotifyIcon_TrayContextMenuOpen(object sender, RoutedEventArgs e)
        {

        }

        private void MyNotifyIcon_PreviewTrayContextMenuOpen(object sender, RoutedEventArgs e)
        {

        }

        private void Open_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Show();
                WindowState = WindowState.Normal;
                ShowInTaskbar = true;
                Activate();
            }
            catch (Exception)
            {

            }
        }
        private async void Exit_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Notification.Bye();
                await Task.Delay(TimeSpan.FromSeconds(2));
                App.Current.Shutdown();
            }
            catch (Exception)
            {

            }
        }

        private void AddDns_Click(object sender, RoutedEventArgs e)
        {
            if (!App.IsUpdating)
                new AddDnsWindow()
                {
                    Owner = this,
                }.ShowDialog();
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.P)
            {
                foreach (var item in App.DnsList)
                {
                    await AppHelpers.TestPing(item);
                }
                e.Handled = true;
            }
            //if (e.Key == Key.S)
            //InkCanvas.SaveToImage(InkCanvas, $"{Random.Shared.Next(1,1000)}.png", ImageFormat.Png);
        }
    }
}