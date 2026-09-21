using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using QisqaTugma.Helpers;

namespace QisqaTugma.Services;

public enum MouseButton
{
    Left,
    Right,
    Middle,
    WheelUp,
    WheelDown
}

public class GlobalMouseHook : IDisposable
{
    private IntPtr _hookId = IntPtr.Zero;
    private readonly NativeMethods.LowLevelMouseProc _proc;
    private bool _disposed;

    public event Action<Point>? MouseMoved;
    public event Action<Point, MouseButton>? MouseClicked;

    public GlobalMouseHook()
    {
        _proc = HookCallback;
    }

    public void Start()
    {
        if (_hookId != IntPtr.Zero) return;

        using var curProcess = Process.GetCurrentProcess();
        using var curModule = curProcess.MainModule!;
        _hookId = NativeMethods.SetWindowsHookEx(
            NativeMethods.WH_MOUSE_LL,
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
            var hookStruct = Marshal.PtrToStructure<NativeMethods.MSLLHOOKSTRUCT>(lParam);
            var point = new Point(hookStruct.pt.X, hookStruct.pt.Y);

            int msg = wParam.ToInt32();

            switch (msg)
            {
                case NativeMethods.WM_MOUSEMOVE:
                    MouseMoved?.Invoke(point);
                    break;
                case NativeMethods.WM_LBUTTONDOWN:
                    MouseClicked?.Invoke(point, MouseButton.Left);
                    break;
                case NativeMethods.WM_RBUTTONDOWN:
                    MouseClicked?.Invoke(point, MouseButton.Right);
                    break;
                case NativeMethods.WM_MBUTTONDOWN:
                    MouseClicked?.Invoke(point, MouseButton.Middle);
                    break;
                case NativeMethods.WM_MOUSEWHEEL:
                    int wheelDelta = (short)(hookStruct.mouseData >> 16);
                    if (wheelDelta > 0)
                        MouseClicked?.Invoke(point, MouseButton.WheelUp);
                    else
                        MouseClicked?.Invoke(point, MouseButton.WheelDown);
                    break;
            }
        }
        return NativeMethods.CallNextHookEx(_hookId, nCode, wParam, lParam);
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

    ~GlobalMouseHook() => Dispose();
}
