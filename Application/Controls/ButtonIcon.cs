using Application.Controls.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Application.Controls
{
    public class ButtonIcon : Button
    {
        static ButtonIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonIcon), new FrameworkPropertyMetadata(typeof(ButtonIcon)));
        }





        public double DistanceBetwewn
        {
            get { return (double)GetValue(DistanceBetwewnProperty); }
            set { SetValue(DistanceBetwewnProperty, value); }
        }
        public static readonly DependencyProperty DistanceBetwewnProperty =
            DependencyProperty.Register(nameof(DistanceBetwewn), typeof(double), typeof(ButtonIcon), new PropertyMetadata(default));




        public ButtonTemplate ButtonTemplate
        {
            get { return (ButtonTemplate)GetValue(ButtonTemplateProperty); }
            set { SetValue(ButtonTemplateProperty, value); }
        }

        public static readonly DependencyProperty ButtonTemplateProperty =
            DependencyProperty.Register(nameof(ButtonTemplate), typeof(ButtonTemplate), typeof(ButtonIcon), new PropertyMetadata(default));

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(ButtonIcon), new PropertyMetadata(default));
    }
}
