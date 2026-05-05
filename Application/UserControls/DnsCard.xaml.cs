using Application.Exceptions;
using Application.Models;
using Application.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Application.UserControls;

public partial class DnsCard : UserControl
{
    public DnsCard()
    {
        InitializeComponent();
        //InitializeAutoScroll();
        this.DataContext = this;
        this.Loaded += DnsCard_Loaded;
    }

    private void DnsCard_Loaded(object sender, RoutedEventArgs e)
    {
        AutoScroll();
    }

    public int Id
    {
        get { return (int)GetValue(IdProperty); }
        set { SetValue(IdProperty, value); }
    }
    public static readonly DependencyProperty IdProperty =
        DependencyProperty.Register(nameof(Id), typeof(int), typeof(DnsCard), new PropertyMetadata(default));


    public Dns Dns
    {
        get { return (Dns)GetValue(DnsProperty); }
        set { SetValue(DnsProperty, value); }
    }
    public static readonly DependencyProperty DnsProperty =
        DependencyProperty.Register(nameof(Dns), typeof(Dns), typeof(DnsCard), new PropertyMetadata(default));



    public DnsControlStyle Resource
    {
        get { return (DnsControlStyle)GetValue(ResourceProperty); }
        set { SetValue(ResourceProperty, value); }
    }

    public static readonly DependencyProperty ResourceProperty =
        DependencyProperty.Register(nameof(Resource), typeof(DnsControlStyle), typeof(DnsCard), new PropertyMetadata(default));



    public void Disconnect()
    {
        btnConnect.Content = "Connect";
        btnConnect.Tag = Tag = "false";
    }


    private void Connect_Click(object sender, RoutedEventArgs e)
    {
        Connect();
    }

    public void Connect()
    {
        try
        {
            if (App.Connect == this)
            {
                Disconnect();
                Notification.Disconnect();
                App.Connect = null;
                App.DnsService.Set(Ip.Epmty);
                return;
            }

            App.OnForceDisconnect(this);
            var result = App.DnsService.Set(Dns.Ip);

            Notification.Connect();

            App.Connect = this;

            btnConnect.Content = "Connected";
            btnConnect.Tag = Tag = "true";
        }
        catch (AccessDeniedException ex)
        {
            Notification.Show(new(ex.Message));
        }
        catch (Exception ex)
        {
            Notification.Show(new(ex.Message));
        }
    }

    void AutoScroll()
    {
        if (tbTitle.ActualWidth > svTitle.ViewportWidth)
        {
            TranslateTransform? transform = tbTitle.RenderTransform as TranslateTransform;
            if (transform != null)
            {
                tbTitle.RenderTransform.Transform(new(svTitle.ViewportWidth, 0));
                double endValue = -tbTitle.ActualWidth;
                TimeSpan duration = TimeSpan.FromSeconds(7);

                DoubleAnimation doubleAnimation = new DoubleAnimation(svTitle.ViewportWidth, endValue, duration, FillBehavior.HoldEnd);

                doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;

                transform.BeginAnimation(TranslateTransform.XProperty, doubleAnimation);
            }
        }
    }
}
