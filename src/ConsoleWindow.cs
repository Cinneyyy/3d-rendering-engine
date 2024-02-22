using System.Runtime.InteropServices;
using System;

namespace src;

public static partial class ConsoleWindow
{
    public enum WindowMode
    {
        Hide = 0,
        Maximize = 3,
        Minimize = 6,
        Restore = 9
    }


    public static readonly IntPtr consoleWindow = GetConsoleWindow();

    private static WindowMode _windowMode = WindowMode.Restore;


    public static WindowMode windowMode
    {
        get => _windowMode;
        set => ShowWindow(_windowMode = value);
    }


    public static bool ShowWindow(WindowMode windowMode) 
    {
        _windowMode = windowMode;
        return ShowWindow(consoleWindow, (int)windowMode); 
    }
    public static bool ShowWindow(int cmdShow) 
        => ShowWindow((WindowMode)cmdShow);

    public static bool Hide() => ShowWindow(WindowMode.Hide);
    public static bool Maximize() => ShowWindow(WindowMode.Maximize);
    public static bool Minimize() => ShowWindow(WindowMode.Minimize);
    public static bool Restore() => ShowWindow(WindowMode.Restore);

    public static void Output(object? o)
    {
        Restore();
        Console.WriteLine(o);
    }

    public static IntPtr PostMessage(ConsoleKey key, uint msg = 0x100u) => PostMessage(consoleWindow, msg, (IntPtr)key, IntPtr.Zero);


    [LibraryImport("kernel32.dll")]
    private static partial IntPtr GetConsoleWindow();

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ShowWindow(IntPtr hWnd, int cmdShow);

    [LibraryImport("user32.dll")]
    private static partial IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
}