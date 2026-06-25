using Application.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Application.Windows
{
    public partial class NotifiactionWindow : Window, INotifyPropertyChanged
    {
        private readonly Rect _window;
        private readonly IEasingFunction _easingFunction;
        private bool _isClosing;
        public event PropertyChangedEventHandler? PropertyChanged;

        private Message message;
        public Message Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                PropertyChanged?.Invoke(this, new(nameof(Message)));
            }
        }

        private ImageSource image;

        public ImageSource Image
        {
            get { return image; }
            set
            {
                image = value;
                PropertyChanged?.Invoke(this, new(nameof(Image)));
            }
        }




        public NotifiactionWindow()
        {
            InitializeComponent();

            this.DataContext = this;
            _window = SystemParameters.WorkArea;
            _easingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut };
        }

        public void Open(Message message)
        {
            Message = message;
            Left = _window.Right - Width - 10;
            Top = _window.Bottom - Height - 30;
            OpenBeginAnimation();
        }

        private void OpenBeginAnimation()
        {
            var slideAnim = new DoubleAnimation(_window.Right, Left, TimeSpan.FromSeconds(0.5))
            {
                EasingFunction = _easingFunction
            };
            BeginAnimation(LeftProperty, slideAnim);
        }
        private void CloseBeginAnimation()
        {
            var slideAnim = new DoubleAnimation(Left, _window.Right, TimeSpan.FromSeconds(0.5))
            {
                EasingFunction = _easingFunction
            };
            BeginAnimation(LeftProperty, slideAnim);
        }

        protected override async void OnClosing(CancelEventArgs e)
        {
            if (_isClosing)
            {
                base.OnClosing(e);
                return;
            }

            e.Cancel = true;
            _isClosing = true;

            await Task.Delay(TimeSpan.FromSeconds(3));
            CloseBeginAnimation();
            await Task.Delay(TimeSpan.FromSeconds(0.5));

            Close();
        }

    }
}
