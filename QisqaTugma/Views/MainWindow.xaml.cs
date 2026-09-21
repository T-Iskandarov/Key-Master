using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using QisqaTugma.Helpers;
using QisqaTugma.Models;
using QisqaTugma.Services;

namespace QisqaTugma.Views;

public partial class MainWindow : Window
{
    private AppSettings _settings;
    private bool _isLoading = true;

    public event Action<AppSettings>? SettingsChanged;

    public MainWindow(AppSettings settings)
    {
        InitializeComponent();
        _settings = settings;

        Loaded += (s, e) => 
        {
            _isLoading = true;
            LoadSettingsToUI();
            UpdateTexts();
            _isLoading = false;
        };

        LocalizationService.LanguageChanged += UpdateTexts;
    }

    private void UpdateTexts()
    {
        Title = LocalizationService.GetString("Settings_Title");

        
        LblLanguage.Text = LocalizationService.GetString("Label_Language");
        LblTextColor.Text = LocalizationService.GetString("Label_TextColor");
        LblBgColor.Text = LocalizationService.GetString("Label_BgColor");
        LblFontSize.Text = LocalizationService.GetString("Label_FontSize");
        LblPosition.Text = LocalizationService.GetString("Label_Position");
        LblDuration.Text = LocalizationService.GetString("Label_Duration");
        
        ShowAllKeysCheck.Content = LocalizationService.GetString("Feature_ShowAll");
        OnlyFunctionalCheck.Content = LocalizationService.GetString("Feature_OnlyFunctional");
        VirtualKeyboardCheck.Content = LocalizationService.GetString("Feature_VirtualKeyboard");
        DoubleCtrlSpotlightCheck.Content = LocalizationService.GetString("Feature_Spotlight");
        KeySoundCheck.Content = LocalizationService.GetString("Feature_KeySound");
        ShowMouseClicksCheck.Content = LocalizationService.GetString("Feature_MouseClicks");
        ClickAnimationCheck.Content = LocalizationService.GetString("Feature_ClickAnimation");
        HotkeyDescCheck.Content = LocalizationService.GetString("Feature_HotkeyDesc");
        
        BtnDatabase.Content = LocalizationService.GetString("Button_Database");
        HeaderFeatures.Text = LocalizationService.GetString("Tab_Features");
        HeaderExtra.Text = LocalizationService.GetString("Header_Extra");
        
        LblAnimType.Text = LocalizationService.GetString("Label_AnimType");
        LblClickColor.Text = LocalizationService.GetString("Label_ClickColor");
        LblSpotlightRadius.Text = LocalizationService.GetString("Label_SpotlightRadius");
        
        BtnSave.Content = LocalizationService.GetString("Button_Save");
        BtnClose.Content = LocalizationService.GetString("Button_Close");

        // Combo box items
        UpdateComboItem(PositionCombo, "TopLeft", "Pos_TopLeft");
        UpdateComboItem(PositionCombo, "TopCenter", "Pos_TopCenter");
        UpdateComboItem(PositionCombo, "TopRight", "Pos_TopRight");
        UpdateComboItem(PositionCombo, "CenterLeft", "Pos_CenterLeft");
        UpdateComboItem(PositionCombo, "Center", "Pos_Center");
        UpdateComboItem(PositionCombo, "CenterRight", "Pos_CenterRight");
        UpdateComboItem(PositionCombo, "BottomLeft", "Pos_BottomLeft");
        UpdateComboItem(PositionCombo, "BottomCenter", "Pos_BottomCenter");
        UpdateComboItem(PositionCombo, "BottomRight", "Pos_BottomRight");

        UpdateComboItem(AnimTypeCombo, "Ripple", "Anim_Ripple");
        UpdateComboItem(AnimTypeCombo, "Shrink", "Anim_Shrink");
        UpdateComboItem(AnimTypeCombo, "Flash", "Anim_Flash");
        UpdateComboItem(AnimTypeCombo, "Ring", "Anim_Ring");
    }

    private void UpdateComboItem(System.Windows.Controls.ComboBox combo, string tag, string key)
    {
        foreach (ComboBoxItem item in combo.Items)
        {
            if (item.Tag?.ToString() == tag)
            {
                item.Content = LocalizationService.GetString(key);
                break;
            }
        }
    }

    private void LoadSettingsToUI()
    {
        // Language
        foreach (ComboBoxItem item in LanguageCombo.Items)
        {
            if (item.Tag?.ToString() == _settings.Language)
            {
                LanguageCombo.SelectedItem = item;
                break;
            }
        }

        // Colors
        TextColorBox.Text = _settings.TextColor;
        TextColorPreview.Background = ColorHelper.BrushFromHex(_settings.TextColor);
        BgColorBox.Text = _settings.BackgroundColor;
        BgColorPreview.Background = ColorHelper.BrushFromHex(_settings.BackgroundColor);
        ClickColorBox.Text = _settings.ClickAnimationColor;
        ClickColorPreview.Background = ColorHelper.BrushFromHex(_settings.ClickAnimationColor);

        // Sliders
        FontSizeSlider.Value = _settings.FontSize;
        FontSizeLabel.Text = _settings.FontSize.ToString("F0");
        DurationSlider.Value = _settings.KeyDisplayDuration;
        DurationLabel.Text = $"{_settings.KeyDisplayDuration:F1}s";
        SpotlightSlider.Value = _settings.SpotlightRadius;
        SpotlightLabel.Text = _settings.SpotlightRadius.ToString("F0");

        // Position
        foreach (ComboBoxItem item in PositionCombo.Items)
        {
            if (item.Tag?.ToString() == _settings.Position)
            {
                PositionCombo.SelectedItem = item;
                break;
            }
        }

        // Checkboxes
        ShowAllKeysCheck.IsChecked = _settings.ShowAllKeys;
        OnlyFunctionalCheck.IsChecked = _settings.ShowOnlyFunctionalKeys;
        VirtualKeyboardCheck.IsChecked = _settings.EnableVirtualKeyboard;
        KeySoundCheck.IsChecked = _settings.EnableKeySound;
        ClickAnimationCheck.IsChecked = _settings.EnableClickAnimation;
        ShowMouseClicksCheck.IsChecked = _settings.ShowMouseClicks;
        DoubleCtrlSpotlightCheck.IsChecked = _settings.EnableDoubleCtrlSpotlight;
        HotkeyDescCheck.IsChecked = _settings.EnableHotkeyDescriptions;

        // Spotlight radius
        SpotlightSlider.Value = _settings.SpotlightRadius;
        SpotlightLabel.Text = _settings.SpotlightRadius.ToString();

        // Click animation
        foreach (ComboBoxItem item in AnimTypeCombo.Items)
        {
            if (item.Tag?.ToString() == _settings.ClickAnimationType)
            {
                AnimTypeCombo.SelectedItem = item;
                break;
            }
        }

        // Theme
        foreach (ComboBoxItem item in ThemeCombo.Items)
        {
            if (item.Tag?.ToString() == _settings.Theme)
            {
                ThemeCombo.SelectedItem = item;
                break;
            }
        }

        ApplyTheme(_settings.Theme);
    }

    private void ApplyTheme(string theme)
    {
        if (theme == "Dark")
        {
            RootGrid.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
            Foreground = Brushes.White;
        }
        else
        {
            RootGrid.Background = Brushes.White;
            Foreground = Brushes.Black;
        }
    }

    // -- Event handlers --

    private void TextColorBox_Changed(object sender, TextChangedEventArgs e)
    {
        if (_isLoading) return;
        _settings.TextColor = TextColorBox.Text;
        try { TextColorPreview.Background = ColorHelper.BrushFromHex(TextColorBox.Text); } catch { }
    }

    private void BgColorBox_Changed(object sender, TextChangedEventArgs e)
    {
        if (_isLoading) return;
        _settings.BackgroundColor = BgColorBox.Text;
        try { BgColorPreview.Background = ColorHelper.BrushFromHex(BgColorBox.Text); } catch { }
    }

    private void ClickColorBox_Changed(object sender, TextChangedEventArgs e)
    {
        if (_isLoading) return;
        _settings.ClickAnimationColor = ClickColorBox.Text;
        try { ClickColorPreview.Background = ColorHelper.BrushFromHex(ClickColorBox.Text); } catch { }
    }

    private void TextColorPreview_Click(object sender, RoutedEventArgs e) =>
        PickColor(TextColorBox, TextColorPreview);

    private void BgColorPreview_Click(object sender, RoutedEventArgs e) =>
        PickColor(BgColorBox, BgColorPreview);

    private void ClickColorPreview_Click(object sender, RoutedEventArgs e) =>
        PickColor(ClickColorBox, ClickColorPreview);

    private void PickColor(TextBox textBox, System.Windows.Controls.Border preview)
    {
        var dialog = new System.Windows.Forms.ColorDialog();
        try
        {
            var currentColor = ColorHelper.FromHex(textBox.Text);
            dialog.Color = System.Drawing.Color.FromArgb(currentColor.A, currentColor.R, currentColor.G, currentColor.B);
        }
        catch { }

        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            var c = dialog.Color;
            var hex = ColorHelper.ToHex(Color.FromArgb(c.A, c.R, c.G, c.B));
            textBox.Text = hex;
            preview.Background = new SolidColorBrush(Color.FromArgb(c.A, c.R, c.G, c.B));
        }
    }

    private void FontSizeSlider_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (_isLoading || FontSizeLabel == null) return;
        _settings.FontSize = FontSizeSlider.Value;
        FontSizeLabel.Text = FontSizeSlider.Value.ToString("F0");
    }

    private void DurationSlider_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (_isLoading || DurationLabel == null) return;
        _settings.KeyDisplayDuration = DurationSlider.Value;
        DurationLabel.Text = $"{DurationSlider.Value:F1}s";
    }

    private void SpotlightSlider_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (_isLoading || SpotlightLabel == null) return;
        _settings.SpotlightRadius = SpotlightSlider.Value;
        SpotlightLabel.Text = SpotlightSlider.Value.ToString("F0");
    }

    private void LanguageCombo_Changed(object sender, SelectionChangedEventArgs e)
    {
        if (_isLoading) return;
        if (LanguageCombo.SelectedItem is ComboBoxItem item)
        {
            _settings.Language = item.Tag?.ToString() ?? "uz";
            LocalizationService.CurrentLanguage = _settings.Language;
            LocalizationService.NotifyLanguageChanged();
            if (Application.Current is App app)
            {
                app.ReloadDictionary();
            }
        }
    }

    private void PositionCombo_Changed(object sender, SelectionChangedEventArgs e)
    {
        if (_isLoading) return;
        if (PositionCombo.SelectedItem is ComboBoxItem item)
            _settings.Position = item.Tag?.ToString() ?? "BottomCenter";
    }

    private void AnimType_Changed(object sender, SelectionChangedEventArgs e)
    {
        if (_isLoading) return;
        if (AnimTypeCombo.SelectedItem is ComboBoxItem item)
            _settings.ClickAnimationType = item.Tag?.ToString() ?? "Ripple";
    }

    private void ThemeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isLoading) return;
        if (ThemeCombo.SelectedItem is ComboBoxItem item)
        {
            _settings.Theme = item.Tag?.ToString() ?? "Light";
            ApplyTheme(_settings.Theme);
        }
    }

    private void FeatureCheck_Changed(object sender, RoutedEventArgs e)
    {
        if (_isLoading) return;
        _settings.ShowAllKeys = ShowAllKeysCheck.IsChecked == true;
        _settings.ShowOnlyFunctionalKeys = OnlyFunctionalCheck.IsChecked == true;
        _settings.EnableVirtualKeyboard = VirtualKeyboardCheck.IsChecked == true;
        _settings.EnableKeySound = KeySoundCheck.IsChecked == true;
        _settings.EnableClickAnimation = ClickAnimationCheck.IsChecked == true;
        _settings.ShowMouseClicks = ShowMouseClicksCheck.IsChecked == true;
        _settings.EnableDoubleCtrlSpotlight = DoubleCtrlSpotlightCheck.IsChecked == true;
        _settings.EnableHotkeyDescriptions = HotkeyDescCheck.IsChecked == true;
    }

    private void OpenDictionary_Click(object sender, RoutedEventArgs e)
    {
        var dictWindow = new HotkeyDictionaryWindow();
        dictWindow.ShowDialog();
        ((App)Application.Current).ReloadDictionary();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        SettingsManager.Save(_settings);
        SettingsChanged?.Invoke(_settings);
        MessageBox.Show(LocalizationService.GetString("Message_Saved"), 
                        LocalizationService.GetString("MsgTitle_Info"), 
                        MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Hide();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }

    private void Banner_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://cubo.uz",
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Saytni ochishda xatolik yuz berdi: {ex.Message}", "Xatolik", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
