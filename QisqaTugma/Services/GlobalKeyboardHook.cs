using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using QisqaTugma.Helpers;
using QisqaTugma.Models;

namespace QisqaTugma.Services;

public class GlobalKeyboardHook : IDisposable
{
    private IntPtr _hookId = IntPtr.Zero;
    private readonly NativeMethods.LowLevelKeyboardProc _proc;
    private bool _disposed;

    public event Action<KeyStroke>? KeyPressed;
    public event Action<KeyStroke>? KeyReleased;

    public GlobalKeyboardHook()
    {
        _proc = HookCallback;
    }

    public void Start()
    {
        if (_hookId != IntPtr.Zero) return;

        using var curProcess = Process.GetCurrentProcess();
        using var curModule = curProcess.MainModule!;
        _hookId = NativeMethods.SetWindowsHookEx(
            NativeMethods.WH_KEYBOARD_LL,
            _proc,
            NativeMethods.GetModuleHandle(curModule.ModuleName),
            0);
    }

    public void Stop()
    {
        if (_hookId != IntPtr.Zero)
        {
            NativeMethods.UnhookWindowsHookEx(_hookId);
            _hookId = IntPtr.Zero;
        }
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            var hookStruct = Marshal.PtrToStructure<NativeMethods.KBDLLHOOKSTRUCT>(lParam);
            var key = KeyInterop.KeyFromVirtualKey(hookStruct.vkCode);
            var modifiers = GetCurrentModifiers();
            var isModifier = IsModifierKey(key);

            var stroke = new KeyStroke
            {
                Key = key,
                VkCode = hookStruct.vkCode,
                Modifiers = modifiers,
                DisplayText = KeyDisplayService.FormatKeyStroke(key, modifiers, isModifier),
                Timestamp = DateTime.Now,
                IsModifierOnly = isModifier
            };

            int msg = wParam.ToInt32();
            if (msg == NativeMethods.WM_KEYDOWN || msg == NativeMethods.WM_SYSKEYDOWN)
            {
                KeyPressed?.Invoke(stroke);
            }
            else if (msg == NativeMethods.WM_KEYUP || msg == NativeMethods.WM_SYSKEYUP)
            {
                KeyReleased?.Invoke(stroke);
            }
        }
        return NativeMethods.CallNextHookEx(_hookId, nCode, wParam, lParam);
    }

    private static ModifierKeys GetCurrentModifiers()
    {
        var modifiers = ModifierKeys.None;
        if ((NativeMethods.GetKeyState(0xA2) & 0x8000) != 0 ||
            (NativeMethods.GetKeyState(0xA3) & 0x8000) != 0)
            modifiers |= ModifierKeys.Control;
        if ((NativeMethods.GetKeyState(0xA0) & 0x8000) != 0 ||
            (NativeMethods.GetKeyState(0xA1) & 0x8000) != 0)
            modifiers |= ModifierKeys.Shift;
        if ((NativeMethods.GetKeyState(0xA4) & 0x8000) != 0 ||
            (NativeMethods.GetKeyState(0xA5) & 0x8000) != 0)
            modifiers |= ModifierKeys.Alt;
        if ((NativeMethods.GetKeyState(0x5B) & 0x8000) != 0 ||
            (NativeMethods.GetKeyState(0x5C) & 0x8000) != 0)
            modifiers |= ModifierKeys.Windows;
        return modifiers;
    }

    private static bool IsModifierKey(Key key)
    {
        return key is Key.LeftCtrl or Key.RightCtrl
            or Key.LeftShift or Key.RightShift
            or Key.LeftAlt or Key.RightAlt
            or Key.LWin or Key.RWin
            or Key.CapsLock or Key.NumLock or Key.Scroll;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Stop();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }

    ~GlobalKeyboardHook() => Dispose();
}
