namespace QisqaTugma.Models;

public class AppSettings
{
    // Display settings
    public string Language { get; set; } = "uz";
    public string TextColor { get; set; } = "#FFFFFF";
    public string BackgroundColor { get; set; } = "#CC000000";
    public double FontSize { get; set; } = 28;
    public string Position { get; set; } = "BottomCenter";
    public double KeyDisplayDuration { get; set; } = 2.0;

    // Feature toggles (checkboxes)
    public bool ShowAllKeys { get; set; } = true;
    public bool ShowOnlyFunctionalKeys { get; set; } = false;
    public bool EnableVirtualKeyboard { get; set; } = true;
    public bool EnableKeySound { get; set; } = true;
    public bool ShowMouseClicks { get; set; } = true;
    public bool EnableDoubleCtrlSpotlight { get; set; } = false;
    public bool EnableClickAnimation { get; set; } = false;
    public bool EnableHotkeyDescriptions { get; set; } = false;

    // Virtual keyboard
    public string VirtualKeyboardHotkey { get; set; } = "Ctrl+F12";

    // Spotlight
    public double SpotlightRadius { get; set; } = 150;

    // Click animation
    public string ClickAnimationType { get; set; } = "Ripple";
    public string ClickAnimationColor { get; set; } = "#FFFF4444";

    // Theme
    public string Theme { get; set; } = "Light";
}
