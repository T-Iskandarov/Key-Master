using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using QisqaTugma.Helpers;

namespace QisqaTugma.Views;

public partial class VirtualKeyboardWindow : Window
{
    private readonly Dictionary<string, Border> _keyElements = new();
    private readonly SolidColorBrush _normalBrush = new(Color.FromArgb(0xFF, 0x55, 0x55, 0x55));
    private readonly SolidColorBrush _pressedBrush = new(Color.FromArgb(0xFF, 0x00, 0x99, 0xFF));
    private readonly SolidColorBrush _textBrush = new(Colors.White);

    public VirtualKeyboardWindow()
    {
        InitializeComponent();
        _normalBrush.Freeze();
        _pressedBrush.Freeze();
        _textBrush.Freeze();
        BuildKeyboard();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var hwnd = new WindowInteropHelper(this).Handle;
        var extStyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE);
        NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE,
            extStyle | NativeMethods.WS_EX_TRANSPARENT | NativeMethods.WS_EX_TOOLWINDOW);

        // Position at bottom center
        var workArea = SystemParameters.WorkArea;
        Left = (workArea.Width - ActualWidth) / 2 + workArea.Left;
        Top = workArea.Bottom - ActualHeight - 40;
    }

    private void BuildKeyboard()
    {
        double kWidth = 46; // base key width
        double kHeight = 40; // base key height

        // --- Asosiy klaviatura ---
        // Row 0: Function keys
        string[] row0 = { "Esc", "GAP", "F1", "F2", "F3", "F4", "GAP", "F5", "F6", "F7", "F8", "GAP", "F9", "F10", "F11", "F12" };
        AddRow(Row0, row0, kWidth, kHeight);

        // Row 1: Number row
        string[] row1 = { "`", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=", "Backspace" };
        double[] w1 = { kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, 90 };
        AddRow(Row1, row1, w1, kHeight);

        // Row 2: QWERTY
        string[] row2 = { "Tab", "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "[", "]", "\\" };
        double[] w2 = { 65, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, 71 };
        AddRow(Row2, row2, w2, kHeight);

        // Row 3: Home row
        string[] row3 = { "CapsLock", "A", "S", "D", "F", "G", "H", "J", "K", "L", ";", "'", "Enter" };
        double[] w3 = { 80, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, 102 };
        AddRow(Row3, row3, w3, kHeight);

        // Row 4: Shift row
        string[] row4 = { "Shift", "Z", "X", "C", "V", "B", "N", "M", ",", ".", "/", "Shift" };
        double[] w4 = { 105, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, kWidth, 123 };
        AddRow(Row4, row4, w4, kHeight);

        // Row 5: Bottom row
        string[] row5 = { "Ctrl", "Win", "Alt", "Space", "Alt", "Win", "Menu", "Ctrl" };
        double[] w5 = { 65, 55, 55, 260, 55, 55, 55, 65 };
        AddRow(Row5, row5, w5, kHeight);

        // --- Navigatsiya qismi ---
        string[] nav0 = { "PrtSc", "ScrLk", "Pause" };
        AddRow(NavRow0, nav0, kWidth, kHeight);

        string[] nav1 = { "Ins", "Home", "PgUp" };
        AddRow(NavRow1, nav1, kWidth, kHeight);

        string[] nav2 = { "Del", "End", "PgDn" };
        AddRow(NavRow2, nav2, kWidth, kHeight);

        string[] nav3 = { "BLANK", "BLANK", "BLANK" };
        AddRow(NavRow3, nav3, kWidth, kHeight);

        string[] nav4 = { "BLANK", "↑", "BLANK" };
        AddRow(NavRow4, nav4, kWidth, kHeight);

        string[] nav5 = { "←", "↓", "→" };
        AddRow(NavRow5, nav5, kWidth, kHeight);

        // --- Numpad qismi ---
        string[] num0 = { "BLANK", "BLANK", "BLANK", "BLANK" };
        AddRow(NumRow0, num0, kWidth, kHeight);

        string[] num1 = { "NumLk", "/", "*", "-" };
        AddRow(NumRow1, num1, kWidth, kHeight);

        string[] num2 = { "7", "8", "9", "+" };
        AddRow(NumRow2, num2, kWidth, kHeight);

        string[] num3 = { "4", "5", "6", "BLANK" }; 
        AddRow(NumRow3, num3, kWidth, kHeight);

        string[] num4 = { "1", "2", "3", "Enter" };
        AddRow(NumRow4, num4, kWidth, kHeight);

        string[] num5 = { "0", ".", "BLANK" };
        double[] nw5 = { kWidth * 2 + 2, kWidth, kWidth }; // 0 spans two cols
        AddRow(NumRow5, num5, nw5, kHeight);
    }

    private void AddRow(WrapPanel panel, string[] keys, double defaultWidth, double height)
    {
        var widths = new double[keys.Length];
        Array.Fill(widths, defaultWidth);
        AddRow(panel, keys, widths, height);
    }

    private void AddRow(WrapPanel panel, string[] keys, double[] widths, double height)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i] == "GAP")
            {
                panel.Children.Add(new Border { Width = 15, Height = height });
                continue;
            }
            if (keys[i] == "BLANK")
            {
                panel.Children.Add(new Border { Width = widths[i], Height = height, Margin = new Thickness(1) });
                continue;
            }

            var text = new TextBlock
            {
                Text = keys[i],
                Foreground = _textBrush,
                FontSize = keys[i].Length > 3 ? 10 : 13,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };

            var border = new Border
            {
                Width = widths[i],
                Height = height,
                Background = _normalBrush,
                CornerRadius = new CornerRadius(4),
                Margin = new Thickness(1),
                Child = text,
                SnapsToDevicePixels = true
            };

            panel.Children.Add(border);

            string keyName = keys[i].ToUpper();
            if (!_keyElements.ContainsKey(keyName))
                _keyElements[keyName] = border;
        }
    }

    public void HighlightKey(Key key)
    {
        Dispatcher.Invoke(() =>
        {
            string keyName = Services.KeyDisplayService.GetKeyDisplayName(key).ToUpper();
            if (_keyElements.TryGetValue(keyName, out var border))
            {
                border.Background = _pressedBrush;

                var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
                timer.Tick += (_, _) =>
                {
                    timer.Stop();
                    border.Background = _normalBrush;
                };
                timer.Start();
            }
        });
    }
}
