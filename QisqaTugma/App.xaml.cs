using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Threading;
using QisqaTugma.Helpers;
using QisqaTugma.Models;
using QisqaTugma.Services;
using QisqaTugma.Views;

namespace QisqaTugma;

public partial class App : Application
{
    // Services
    private GlobalKeyboardHook? _keyboardHook;
    private GlobalMouseHook? _mouseHook;
    private AudioService? _audioService;

    // Windows
    private OverlayWindow? _overlayWindow;
    private VirtualKeyboardWindow? _virtualKeyboard;
    private MouseSpotlightWindow? _spotlight;
    private ClickAnimationWindow? _clickAnimation;
    private MainWindow? _mainWindow;

    // System tray
    private System.Windows.Forms.NotifyIcon? _trayIcon;
    private System.Windows.Forms.ToolStripMenuItem? _traySettingsItem;
    private System.Windows.Forms.ToolStripMenuItem? _trayExitItem;

    // Settings
    private AppSettings _settings = new();
    private HotkeyDictionaryData _dictionaryData = new();

    // Double-Ctrl detection
    private DateTime _lastCtrlTime = DateTime.MinValue;
    private const int DoubleClickMs = 400;

    // Virtual keyboard hotkey tracking
    private bool _ctrlHeld;

    private Mutex? _mutex;

    public void ReloadDictionary()
    {
        _dictionaryData = DictionaryService.Load();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        bool createdNew;
        _mutex = new Mutex(true, "KeyMasterApp_Unique_Mutex_123", out createdNew);

        if (!createdNew)
        {
            System.Windows.MessageBox.Show("Dastur allaqachon ishga tushirilgan! Iltimos ekranning o'ng pastki burchagidagi (soat yonidagi) ikonkalarni tekshiring.", "Key Master", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            Application.Current.Shutdown();
            return;
        }

        base.OnStartup(e);

        // Load settings
        _settings = SettingsManager.Load();
        LocalizationService.CurrentLanguage = _settings.Language;
        
        _dictionaryData = DictionaryService.Load();

        // Initialize services
        _audioService = new AudioService();
        _keyboardHook = new GlobalKeyboardHook();
        _mouseHook = new GlobalMouseHook();

        // Initialize windows
        _overlayWindow = new OverlayWindow();
        _overlayWindow.Show();
        _overlayWindow.UpdatePosition(_settings);

        _clickAnimation = new ClickAnimationWindow();
        _clickAnimation.Show();

        _spotlight = new MouseSpotlightWindow();
        _spotlight.UpdateSettings(_settings);

        // Hook events
        _keyboardHook.KeyPressed += OnKeyPressed;
        _keyboardHook.KeyReleased += OnKeyReleased;
        _mouseHook.MouseMoved += OnMouseMoved;
        _mouseHook.MouseClicked += OnMouseClicked;

        LocalizationService.LanguageChanged += UpdateTrayMenuText;

        // Start hooks
        _keyboardHook.Start();
        _mouseHook.Start();

        // Setup system tray
        SetupTrayIcon();
        
        // Show settings window on startup since we don't start with Windows automatically
        ShowSettings();
    }

    private void SetupTrayIcon()
    {
        _trayIcon = new System.Windows.Forms.NotifyIcon
        {
            Icon = new System.Drawing.Icon(Application.GetResourceStream(new Uri("pack://application:,,,/Resources/app.ico")).Stream),
            Text = "Key Master",
            Visible = true
        };

        var menu = new System.Windows.Forms.ContextMenuStrip();
        
        _traySettingsItem = new System.Windows.Forms.ToolStripMenuItem(LocalizationService.GetString("Tray_Settings"));
        _traySettingsItem.Click += (_, _) => ShowSettings();
        
        _trayExitItem = new System.Windows.Forms.ToolStripMenuItem(LocalizationService.GetString("Tray_Exit"));
        _trayExitItem.Click += (_, _) => ExitApp();

        menu.Items.Add(_traySettingsItem);
        menu.Items.Add(new System.Windows.Forms.ToolStripSeparator());
        menu.Items.Add(_trayExitItem);

        _trayIcon.ContextMenuStrip = menu;
        _trayIcon.DoubleClick += (_, _) => ShowSettings();
    }

    private void UpdateTrayMenuText()
    {
        if (_traySettingsItem != null) _traySettingsItem.Text = LocalizationService.GetString("Tray_Settings");
        if (_trayExitItem != null) _trayExitItem.Text = LocalizationService.GetString("Tray_Exit");
    }

    private void ShowSettings()
    {
        if (_mainWindow == null || !_mainWindow.IsLoaded)
        {
            _mainWindow = new MainWindow(_settings);
            _mainWindow.SettingsChanged += OnSettingsChanged;
        }

        _mainWindow.Show();
        _mainWindow.Activate();
    }

    private void OnSettingsChanged(AppSettings newSettings)
    {
        _settings = newSettings;
        _overlayWindow?.UpdatePosition(_settings);

        // Update spotlight
        if (_spotlight == null || !_spotlight.IsLoaded)
        {
            _spotlight = new MouseSpotlightWindow();
        }
        _spotlight.UpdateSettings(_settings);

        if (!_settings.EnableDoubleCtrlSpotlight)
        {
            _spotlight?.Hide();
        }

        // Update virtual keyboard
        if (!_settings.EnableVirtualKeyboard)
        {
            _virtualKeyboard?.Hide();
        }
    }

    private void OnKeyPressed(KeyStroke stroke)
    {
        Dispatcher.Invoke(() =>
        {
            // Check for virtual keyboard hotkey (Ctrl+F12)
            if (stroke.Key == Key.LeftCtrl || stroke.Key == Key.RightCtrl)
                _ctrlHeld = true;

            if (stroke.Key == Key.F12 && _ctrlHeld && _settings.EnableVirtualKeyboard)
            {
                ToggleVirtualKeyboard();
                return;
            }

            // Check for double-Ctrl spotlight
            if (_settings.EnableDoubleCtrlSpotlight &&
                (stroke.Key == Key.LeftCtrl || stroke.Key == Key.RightCtrl))
            {
                var now = DateTime.Now;
                if ((now - _lastCtrlTime).TotalMilliseconds < DoubleClickMs)
                {
                    ToggleSpotlight();
                    _lastCtrlTime = DateTime.MinValue;
                }
                else
                {
                    _lastCtrlTime = now;
                }
            }

            // Determine if should display
            bool shouldDisplay = false;

            if (_settings.ShowAllKeys)
            {
                shouldDisplay = true;
            }
            else if (_settings.ShowOnlyFunctionalKeys)
            {
                shouldDisplay = KeyDisplayService.IsFunctionalKey(stroke.Key) ||
                                stroke.Modifiers != ModifierKeys.None;
            }

            if (shouldDisplay)
            {
                string desc = "";
                if (_settings.EnableHotkeyDescriptions)
                {
                    desc = DictionaryService.GetDescriptionForKey(stroke.DisplayText, _dictionaryData);
                }
                _overlayWindow?.ShowKey(stroke.DisplayText, desc, _settings);
            }

            // Virtual keyboard highlight
            if (_virtualKeyboard?.IsVisible == true)
            {
                _virtualKeyboard.HighlightKey(stroke.Key);
            }

            // Key sound
            if (_settings.EnableKeySound)
            {
                _audioService?.PlayKeySound();
            }
        });
    }

    private void OnKeyReleased(KeyStroke stroke)
    {
        Dispatcher.Invoke(() =>
        {
            if (stroke.Key == Key.LeftCtrl || stroke.Key == Key.RightCtrl)
                _ctrlHeld = false;
        });
    }

    private void OnMouseMoved(System.Windows.Point point)
    {
        if (_spotlight?.IsVisible == true)
        {
            _spotlight.MoveTo(point);
        }
    }

    private void OnMouseClicked(System.Windows.Point point, Services.MouseButton button)
    {
        if (_settings.EnableClickAnimation)
        {
            Dispatcher.Invoke(() =>
            {
                _clickAnimation?.PlayAnimation(point, _settings.ClickAnimationType, _settings.ClickAnimationColor);
            });
        }

        if (_settings.ShowMouseClicks)
        {
            Dispatcher.Invoke(() =>
            {
                string buttonText = button switch
                {
                    Services.MouseButton.Left => LocalizationService.GetString("Mouse_Left"),
                    Services.MouseButton.Right => LocalizationService.GetString("Mouse_Right"),
                    Services.MouseButton.Middle => LocalizationService.GetString("Mouse_Middle"),
                    Services.MouseButton.WheelUp => LocalizationService.GetString("Mouse_WheelUp"),
                    Services.MouseButton.WheelDown => LocalizationService.GetString("Mouse_WheelDown"),
                    _ => ""
                };
                
                if (!string.IsNullOrEmpty(buttonText))
                {
                    _overlayWindow?.ShowKey(buttonText, "", _settings);
                }
            });
        }
    }

    private void ToggleSpotlight()
    {
        if (_spotlight == null || !_spotlight.IsLoaded)
        {
            _spotlight = new MouseSpotlightWindow();
            _spotlight.UpdateSettings(_settings);
        }

        if (_spotlight.IsVisible)
        {
            _spotlight.Hide();
        }
        else
        {
            NativeMethods.GetCursorPos(out var pt);
            _spotlight.MoveTo(new System.Windows.Point(pt.X, pt.Y));
            _spotlight.Show();
        }
    }

    private void ToggleVirtualKeyboard()
    {
        if (_virtualKeyboard == null || !_virtualKeyboard.IsLoaded)
        {
            _virtualKeyboard = new VirtualKeyboardWindow();
        }

        if (_virtualKeyboard.IsVisible)
        {
            _virtualKeyboard.Hide();
        }
        else
        {
            _virtualKeyboard.Show();
        }
    }

    private void ExitApp()
    {
        // Cleanup
        _keyboardHook?.Dispose();
        _mouseHook?.Dispose();
        _audioService?.Dispose();

        _trayIcon?.Dispose();

        _overlayWindow?.Close();
        _virtualKeyboard?.Close();
        _spotlight?.Close();
        _clickAnimation?.Close();
        _mainWindow?.Close();

        Shutdown();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _trayIcon?.Dispose();
        base.OnExit(e);
    }
}
