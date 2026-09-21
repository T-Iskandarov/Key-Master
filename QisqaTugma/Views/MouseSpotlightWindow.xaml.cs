using System.Windows;
using System.Windows.Media;
using QisqaTugma.Models;

namespace QisqaTugma.Views;

public partial class MouseSpotlightWindow : Window
{
    private double _radius = 150;
    
    public MouseSpotlightWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        ScreenRect.Rect = new Rect(
            SystemParameters.VirtualScreenLeft,
            SystemParameters.VirtualScreenTop,
            SystemParameters.VirtualScreenWidth,
            SystemParameters.VirtualScreenHeight);
            
        this.Left = SystemParameters.VirtualScreenLeft;
        this.Top = SystemParameters.VirtualScreenTop;
        this.Width = SystemParameters.VirtualScreenWidth;
        this.Height = SystemParameters.VirtualScreenHeight;
    }

    public void MoveTo(Point p)
    {
        double localX = p.X - SystemParameters.VirtualScreenLeft;
        double localY = p.Y - SystemParameters.VirtualScreenTop;

        SpotlightHole.Center = new Point(localX, localY);
    }

    public void UpdateSettings(AppSettings settings)
    {
        _radius = settings.SpotlightRadius;
        SpotlightHole.RadiusX = _radius;
        SpotlightHole.RadiusY = _radius;
    }
}
