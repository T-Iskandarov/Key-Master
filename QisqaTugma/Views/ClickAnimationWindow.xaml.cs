using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using QisqaTugma.Helpers;

namespace QisqaTugma.Views;

public partial class ClickAnimationWindow : Window
{
    public ClickAnimationWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var hwnd = new WindowInteropHelper(this).Handle;
        var extStyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE);
        NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE,
            extStyle | NativeMethods.WS_EX_TRANSPARENT | NativeMethods.WS_EX_TOOLWINDOW);
    }

    public void PlayAnimation(Point screenPos, string animationType, string colorHex)
    {
        Dispatcher.Invoke(() =>
        {
            Left = screenPos.X - Width / 2;
            Top = screenPos.Y - Height / 2;

            var color = ColorHelper.FromHex(colorHex);
            var brush = new SolidColorBrush(color);

            switch (animationType)
            {
                case "Ripple":
                    PlayRipple(brush);
                    break;
                case "Shrink":
                    PlayShrink(brush);
                    break;
                case "Flash":
                    PlayFlash(brush);
                    break;
                case "Ring":
                    PlayRing(brush);
                    break;
                default:
                    PlayRipple(brush);
                    break;
            }
        });
    }

    private void PlayRipple(SolidColorBrush brush)
    {
        RippleEllipse.Stroke = brush;
        RippleEllipse.Visibility = Visibility.Visible;
        RippleEllipse.Opacity = 1;
        RippleEllipse.Width = 10;
        RippleEllipse.Height = 10;

        var sb = new Storyboard();

        var sizeW = new DoubleAnimation(10, 80, TimeSpan.FromMilliseconds(400))
        { EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut } };
        Storyboard.SetTarget(sizeW, RippleEllipse);
        Storyboard.SetTargetProperty(sizeW, new PropertyPath("Width"));
        sb.Children.Add(sizeW);

        var sizeH = new DoubleAnimation(10, 80, TimeSpan.FromMilliseconds(400))
        { EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut } };
        Storyboard.SetTarget(sizeH, RippleEllipse);
        Storyboard.SetTargetProperty(sizeH, new PropertyPath("Height"));
        sb.Children.Add(sizeH);

        var fade = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(400));
        Storyboard.SetTarget(fade, RippleEllipse);
        Storyboard.SetTargetProperty(fade, new PropertyPath("Opacity"));
        sb.Children.Add(fade);

        sb.Completed += (_, _) => RippleEllipse.Visibility = Visibility.Collapsed;
        sb.Begin();
    }

    private void PlayShrink(SolidColorBrush brush)
    {
        RippleEllipse.Stroke = brush;
        RippleEllipse.Visibility = Visibility.Visible;
        RippleEllipse.Opacity = 1;
        RippleEllipse.Width = 80;
        RippleEllipse.Height = 80;

        var sb = new Storyboard();

        var sizeW = new DoubleAnimation(80, 5, TimeSpan.FromMilliseconds(350))
        { EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn } };
        Storyboard.SetTarget(sizeW, RippleEllipse);
        Storyboard.SetTargetProperty(sizeW, new PropertyPath("Width"));
        sb.Children.Add(sizeW);

        var sizeH = new DoubleAnimation(80, 5, TimeSpan.FromMilliseconds(350))
        { EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn } };
        Storyboard.SetTarget(sizeH, RippleEllipse);
        Storyboard.SetTargetProperty(sizeH, new PropertyPath("Height"));
        sb.Children.Add(sizeH);

        var fade = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(350));
        fade.BeginTime = TimeSpan.FromMilliseconds(200);
        Storyboard.SetTarget(fade, RippleEllipse);
        Storyboard.SetTargetProperty(fade, new PropertyPath("Opacity"));
        sb.Children.Add(fade);

        sb.Completed += (_, _) => RippleEllipse.Visibility = Visibility.Collapsed;
        sb.Begin();
    }

    private void PlayFlash(SolidColorBrush brush)
    {
        FlashEllipse.Fill = brush;
        FlashEllipse.Visibility = Visibility.Visible;

        var sb = new Storyboard();

        var fadeIn = new DoubleAnimation(0, 0.8, TimeSpan.FromMilliseconds(80));
        Storyboard.SetTarget(fadeIn, FlashEllipse);
        Storyboard.SetTargetProperty(fadeIn, new PropertyPath("Opacity"));
        sb.Children.Add(fadeIn);

        var fadeOut = new DoubleAnimation(0.8, 0, TimeSpan.FromMilliseconds(250));
        fadeOut.BeginTime = TimeSpan.FromMilliseconds(80);
        Storyboard.SetTarget(fadeOut, FlashEllipse);
        Storyboard.SetTargetProperty(fadeOut, new PropertyPath("Opacity"));
        sb.Children.Add(fadeOut);

        sb.Completed += (_, _) => FlashEllipse.Visibility = Visibility.Collapsed;
        sb.Begin();
    }

    private void PlayRing(SolidColorBrush brush)
    {
        RingEllipse.Stroke = brush;
        RingEllipse.Visibility = Visibility.Visible;
        RingEllipse.Width = 50;
        RingEllipse.Height = 50;

        var sb = new Storyboard();

        var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(100));
        Storyboard.SetTarget(fadeIn, RingEllipse);
        Storyboard.SetTargetProperty(fadeIn, new PropertyPath("Opacity"));
        sb.Children.Add(fadeIn);

        var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(400));
        fadeOut.BeginTime = TimeSpan.FromMilliseconds(100);
        Storyboard.SetTarget(fadeOut, RingEllipse);
        Storyboard.SetTargetProperty(fadeOut, new PropertyPath("Opacity"));
        sb.Children.Add(fadeOut);

        var thick = new DoubleAnimation(4, 1, TimeSpan.FromMilliseconds(500));
        Storyboard.SetTarget(thick, RingEllipse);
        Storyboard.SetTargetProperty(thick, new PropertyPath("StrokeThickness"));
        sb.Children.Add(thick);

        var sizeW = new DoubleAnimation(50, 90, TimeSpan.FromMilliseconds(500));
        Storyboard.SetTarget(sizeW, RingEllipse);
        Storyboard.SetTargetProperty(sizeW, new PropertyPath("Width"));
        sb.Children.Add(sizeW);

        var sizeH = new DoubleAnimation(50, 90, TimeSpan.FromMilliseconds(500));
        Storyboard.SetTarget(sizeH, RingEllipse);
        Storyboard.SetTargetProperty(sizeH, new PropertyPath("Height"));
        sb.Children.Add(sizeH);

        sb.Completed += (_, _) => RingEllipse.Visibility = Visibility.Collapsed;
        sb.Begin();
    }
}
