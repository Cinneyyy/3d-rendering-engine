using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace src;

public static class Input
{
    private static readonly Dictionary<Keys, bool> keyState = [];
    private static readonly List<Keys> keyDowns = [], keyUps = [];

    private static Vec2i lastCursor;

    public static bool lockCursor { get; set; }


    public static bool KeyHelt(Keys key) => keyState.TryGetValue(key, out var pressed) && pressed;
    public static bool KeyUp(Keys key) => keyState.TryGetValue(key, out var pressed) && !pressed;
    public static bool KeyDown(Keys key) => keyUps.Contains(key);

    public static void FinishTick(object? sender, ElapsedEventArgs args)
    {
        keyDowns.Clear();
        keyUps.Clear();
    }

    public static void Init()
    {
        lastCursor = Window.curr!.center;
    }


    internal static void OnKeyDown(object? sender, KeyEventArgs args)
    {
        Keys key = args.KeyCode;

        if(keyState.TryGetValue(key, out var down) && !down)
            keyDowns.Add(key);

        keyState[key] = true;
    }

    internal static void OnKeyUp(object? sender, KeyEventArgs args)
    {
        Keys key = args.KeyCode;

        if(keyState.TryGetValue(key, out var down) && down)
            keyUps.Add(key);

        keyState[key] = false;
    }

    internal static void OnMouseClick(object? sender, MouseEventArgs args)
    {
        // Uncomment when it is used
        //int mb = args.Button switch {
        //    MouseButtons.Left => 0,
        //    MouseButtons.Right => 1,
        //    MouseButtons.Middle => 2,
        //    _ => -1
        //};

        //if(mb == -1)
        //    return;

        //Vec2 loc = new Vec2(args.X, args.Y) * (Renderer.Screen / Window.curr.size);
    }

    internal static void OnMouseMove(object? sender, MouseEventArgs args)
    {
        if(args.Location == (Point)Window.curr!.center)
            return;

        //Vec2i screenDelta = lastCursor - (Vec2i)args.Location;
        //if(lockCursor) 
        //    Cursor.Position = (Point)Window.curr!.center;
        //lastCursor = (Vec2i)Cursor.Position;
    }
}