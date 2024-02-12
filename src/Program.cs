using System;

namespace src;

public class Program
{
    public static readonly DateTime startupTime = DateTime.Now;

    public static float secondsPassed => (float)(DateTime.Now - startupTime).TotalSeconds;


    private static void Main()
    {
        try
        {
            ConsoleWindow.Hide();
            SpriteLoader.Load();

            //Vec2 physicalScreen = new(Screen.PrimaryScreen?.Bounds.Width ?? Renderer.ScreenW, Screen.PrimaryScreen?.Bounds.Height ?? Renderer.ScreenH);
            //float winScale = Window.ScreenCoverage / 100f * (physicalScreen.y / Renderer.ScreenH);
            //Vec2i virtualScreen = new((int)Math.Ceiling(Renderer.ScreenW * winScale), (int)Math.Ceiling(Renderer.ScreenH * winScale));

            Window win = new("3D Renderer", 60);

            Input.Init();

            win.tick += ProcessState.Tick!;
            win.tick += Renderer.Tick!;
            win.tick += Input.FinishTick!;
        }
        catch(Exception exc)
        {
            Out(exc);
            Console.ReadKey();
        }
    }
}
