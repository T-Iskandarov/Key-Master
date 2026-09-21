using System.Windows.Input;

namespace QisqaTugma.Services;

public static class KeyDisplayService
{
    private static readonly HashSet<Key> FunctionalKeys = new()
    {
        Key.LeftCtrl, Key.RightCtrl,
        Key.LeftShift, Key.RightShift,
        Key.LeftAlt, Key.RightAlt,
        Key.LWin, Key.RWin,
        Key.CapsLock, Key.NumLock, Key.Scroll,
        Key.Tab, Key.Escape, Key.Enter, Key.Return,
        Key.Back, Key.Delete, Key.Insert,
        Key.Home, Key.End, Key.PageUp, Key.PageDown,
        Key.PrintScreen, Key.Pause,
        Key.F1, Key.F2, Key.F3, Key.F4, Key.F5, Key.F6,
        Key.F7, Key.F8, Key.F9, Key.F10, Key.F11, Key.F12,
        Key.Up, Key.Down, Key.Left, Key.Right,
        Key.Apps, Key.Sleep
    };

    public static bool IsFunctionalKey(Key key)
    {
        return FunctionalKeys.Contains(key);
    }

    public static string FormatKeyStroke(Key key, ModifierKeys modifiers, bool isModifierOnly)
    {
        var parts = new List<string>();

        // Add modifiers (but not if the key itself is that modifier)
        if (modifiers.HasFlag(ModifierKeys.Control) && key is not (Key.LeftCtrl or Key.RightCtrl))
            parts.Add("Ctrl");
        if (modifiers.HasFlag(ModifierKeys.Alt) && key is not (Key.LeftAlt or Key.RightAlt))
            parts.Add("Alt");
        if (modifiers.HasFlag(ModifierKeys.Shift) && key is not (Key.LeftShift or Key.RightShift))
            parts.Add("Shift");
        if (modifiers.HasFlag(ModifierKeys.Windows) && key is not (Key.LWin or Key.RWin))
            parts.Add("Win");

        // Add key name
        parts.Add(GetKeyDisplayName(key));

        return string.Join(" + ", parts);
    }

    public static string GetKeyDisplayName(Key key)
    {
        return key switch
        {
            Key.LeftCtrl or Key.RightCtrl => "Ctrl",
            Key.LeftShift or Key.RightShift => "Shift",
            Key.LeftAlt or Key.RightAlt => "Alt",
            Key.LWin or Key.RWin => "Win",
            Key.CapsLock => "CapsLock",
            Key.NumLock => "NumLock",
            Key.Scroll => "ScrollLock",
            Key.Back => "Backspace",
            Key.Return or Key.Enter => "Enter",
            Key.Escape => "Esc",
            Key.Tab => "Tab",
            Key.Space => "Space",
            Key.Delete => "Del",
            Key.Insert => "Ins",
            Key.Home => "Home",
            Key.End => "End",
            Key.PageUp => "PgUp",
            Key.PageDown => "PgDn",
            Key.PrintScreen => "PrtSc",
            Key.Pause => "Pause",
            Key.Up => "↑",
            Key.Down => "↓",
            Key.Left => "←",
            Key.Right => "→",
            Key.Apps => "Menu",
            Key.OemPlus => "+",
            Key.OemMinus => "-",
            Key.OemPeriod => ".",
            Key.OemComma => ",",
            Key.Oem1 => ";",
            Key.Oem2 => "/",
            Key.Oem3 => "`",
            Key.Oem4 => "[",
            Key.Oem5 => "\\",
            Key.Oem6 => "]",
            Key.Oem7 => "'",
            Key.Multiply => "*",
            Key.Add => "+",
            Key.Subtract => "-",
            Key.Divide => "/",
            Key.Decimal => ".",
            // D0-D9 (top row numbers)
            Key.D0 => "0",
            Key.D1 => "1",
            Key.D2 => "2",
            Key.D3 => "3",
            Key.D4 => "4",
            Key.D5 => "5",
            Key.D6 => "6",
            Key.D7 => "7",
            Key.D8 => "8",
            Key.D9 => "9",
            // Numpad
            Key.NumPad0 => "Num0",
            Key.NumPad1 => "Num1",
            Key.NumPad2 => "Num2",
            Key.NumPad3 => "Num3",
            Key.NumPad4 => "Num4",
            Key.NumPad5 => "Num5",
            Key.NumPad6 => "Num6",
            Key.NumPad7 => "Num7",
            Key.NumPad8 => "Num8",
            Key.NumPad9 => "Num9",
            _ => key.ToString()
        };
    }
}
