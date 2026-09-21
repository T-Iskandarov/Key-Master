using System.Windows.Media;

namespace QisqaTugma.Helpers;

public static class ColorHelper
{
    public static Color FromHex(string hex)
    {
        try
        {
            return (Color)ColorConverter.ConvertFromString(hex);
        }
        catch
        {
            return Colors.White;
        }
    }

    public static SolidColorBrush BrushFromHex(string hex)
    {
        var brush = new SolidColorBrush(FromHex(hex));
        brush.Freeze();
        return brush;
    }

    public static string ToHex(Color color)
    {
        return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    public static string ToHexRgb(Color color)
    {
        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }
}
