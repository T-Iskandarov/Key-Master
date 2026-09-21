using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using QisqaTugma.Helpers;
using QisqaTugma.Models;

namespace QisqaTugma.Views;

public partial class OverlayWindow : Window
{
    private readonly DispatcherTimer _hideTimer;
    private Storyboard? _showStoryboard;
    private Storyboard? _hideStoryboard;

    public OverlayWindow()
    {
        InitializeComponent();
        _hideTimer = new DispatcherTimer();
        _hideTimer.Tick += HideTimer_Tick;
        CreateAnimations();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // Make window click-through
        var hwnd = new WindowInteropHelper(this).Handle;
        var extStyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE);
        NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE,
            extStyle | NativeMethods.WS_EX_TRANSPARENT | NativeMethods.WS_EX_TOOLWINDOW);
    }

    private void CreateAnimations()
    {
        // Show animation: scale from 0.8 to 1.0 + fade in
        _showStoryboard = new Storyboard();

        var scaleXAnim = new DoubleAnimation(0.85, 1.0, TimeSpan.FromMilliseconds(120))
        {
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
        };
        Storyboard.SetTarget(scaleXAnim, KeyBorder);
        Storyboard.SetTargetProperty(scaleXAnim, new PropertyPath("RenderTransform.ScaleX"));
        _showStoryboard.Children.Add(scaleXAnim);

        var scaleYAnim = new DoubleAnimation(0.85, 1.0, TimeSpan.FromMilliseconds(120))
        {
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
        };
        Storyboard.SetTarget(scaleYAnim, KeyBorder);
        Storyboard.SetTargetProperty(scaleYAnim, new PropertyPath("RenderTransform.ScaleY"));
        _showStoryboard.Children.Add(scaleYAnim);

        var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(100));
        Storyboard.SetTarget(fadeIn, KeyBorder);
        Storyboard.SetTargetProperty(fadeIn, new PropertyPath("Opacity"));
        _showStoryboard.Children.Add(fadeIn);

        var descFadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(100));
        Storyboard.SetTarget(descFadeIn, DescBorder);
        Storyboard.SetTargetProperty(descFadeIn, new PropertyPath("Opacity"));
        _showStoryboard.Children.Add(descFadeIn);

        // Hide animation: fade out
        _hideStoryboard = new Storyboard();
        // Animations for KeyBorder are already added above...
        var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300))
        {
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
        };
        Storyboard.SetTarget(fadeOut, KeyBorder);
        Storyboard.SetTargetProperty(fadeOut, new PropertyPath("Opacity"));
        _hideStoryboard.Children.Add(fadeOut);

        var descFadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300))
        {
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
        };
        Storyboard.SetTarget(descFadeOut, DescBorder);
        Storyboard.SetTargetProperty(descFadeOut, new PropertyPath("Opacity"));
        _hideStoryboard.Children.Add(descFadeOut);

        _hideStoryboard.Completed += (_, _) => 
        {
            KeyBorder.Visibility = Visibility.Collapsed;
            DescBorder.Visibility = Visibility.Collapsed;
        };
    }

    public void ShowKey(string displayText, string description, AppSettings settings)
    {
        Dispatcher.Invoke(() =>
        {
            _hideTimer.Stop();
            _hideStoryboard?.Stop();

            // Apply settings
            KeyText.Text = displayText;
            KeyText.FontSize = settings.FontSize;
            KeyText.Foreground = ColorHelper.BrushFromHex(settings.TextColor);
            KeyBorder.Background = ColorHelper.BrushFromHex(settings.BackgroundColor);

            KeyBorder.Visibility = Visibility.Visible;
            KeyBorder.Opacity = 1;

            if (!string.IsNullOrEmpty(description))
            {
                DescText.Text = description;
                DescBorder.Visibility = Visibility.Visible;
                DescBorder.Opacity = 1;
            }
            else
            {
                DescBorder.Visibility = Visibility.Collapsed;
            }

            // Start show animation
            _showStoryboard?.Begin();

            // Set hide timer
            _hideTimer.Interval = TimeSpan.FromSeconds(settings.KeyDisplayDuration);
            _hideTimer.Start();
        });
    }

    private void HideTimer_Tick(object? sender, EventArgs e)
    {
        _hideTimer.Stop();
        _hideStoryboard?.Begin();
    }

    public void UpdatePosition(AppSettings settings)
    {
        Dispatcher.Invoke(() =>
        {
            var workArea = SystemParameters.WorkArea;
            double margin = 20;

            switch (settings.Position)
            {
                case "TopLeft":
                    Left = workArea.Left + margin;
                    Top = workArea.Top + margin;
                    break;
                case "TopCenter":
                    Left = workArea.Left + (workArea.Width - Width) / 2;
                    Top = workArea.Top + margin;
                    break;
                case "TopRight":
                    Left = workArea.Right - Width - margin;
                    Top = workArea.Top + margin;
                    break;
                case "CenterLeft":
                    Left = workArea.Left + margin;
                    Top = workArea.Top + (workArea.Height - Height) / 2;
                    break;
                case "Center":
                    Left = workArea.Left + (workArea.Width - Width) / 2;
                    Top = workArea.Top + (workArea.Height - Height) / 2;
                    break;
                case "CenterRight":
                    Left = workArea.Right - Width - margin;
                    Top = workArea.Top + (workArea.Height - Height) / 2;
                    break;
                case "BottomLeft":
                    Left = workArea.Left + margin;
                    Top = workArea.Bottom - Height - margin;
                    break;
                case "BottomCenter":
                    Left = workArea.Left + (workArea.Width - Width) / 2;
                    Top = workArea.Bottom - Height - margin;
                    break;
                case "BottomRight":
                    Left = workArea.Right - Width - margin;
                    Top = workArea.Bottom - Height - margin;
                    break;
            }
        });
    }
}
