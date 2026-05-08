using Application.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Application.Windows
{
    /// <summary>
    /// Interaction logic for DesignCardWindow.xaml
    /// </summary>
    public partial class DesignCardWindow : Window, INotifyPropertyChanged
    {
        public DesignCardWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            Resource = new()
            {
                Character = "1.png",
                Symbol = "heart1.png",
                Colors = new()
                {
                    new(),
                    new(),
                    new()
                },
                BorderColor = "#FF9999"
            };
        }

        private DnsControlStyle resource;

        public DnsControlStyle Resource
        {
            get { return resource; }
            set
            {
                resource = value;
                PropertyChanged?.Invoke(this, new(nameof(Resource)));
            }
        }

        private DnsCardModel dns;

        public DnsCardModel Dns
        {
            get { return dns; }
            set
            {
                dns = value;
                PropertyChanged?.Invoke(this, new(nameof(Dns)));
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
