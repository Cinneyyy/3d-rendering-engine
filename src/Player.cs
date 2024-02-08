using System;
using System.Timers;
using System.Windows.Forms;

namespace src;

#pragma warning disable CA2211 // Non-constant fields should not be visible
public static class Player
{
    public const int TileSize = 16;
    public const int MapX = 8, MapY = 8;

    public static readonly int mapDiameter = (int)MathF.Ceiling(MathF.Sqrt(MapX*MapX + MapY*MapY));

    public static Vec2f pos = new(MapX/2 * TileSize, MapY/2 * TileSize);
    public static float speed = 25f;
    public static float sensitivity { get; set; } = 0.01f;

    private static float _angle = 3f/2f * MathF.PI;


    public static Vec2f forward => Vec2f.FromAngle(angle);
    public static Vec2f right => Vec2f.FromAngle(angle + MathF.PI/2f);
    public static float angle
    {
        get => _angle;
        set {
            while(value < 0) 
                value += 2 * MathF.PI;
            _angle = value % (2 * MathF.PI);
        }
    }


    public static void Tick(object? sender, ElapsedEventArgs args)
    {
        if(Input.KeyDown(Keys.Escape))
        {
            Window.Exit();
            return;
        }

        if(Input.KeyDown(Keys.C))
            ConsoleWindow.windowMode = ConsoleWindow.windowMode == ConsoleWindow.WindowMode.Hide ? ConsoleWindow.WindowMode.Restore : ConsoleWindow.WindowMode.Hide;

        if(Input.KeyDown(Keys.F2))
            Input.lockCursor ^= true;

        if(Input.KeyHelt(Keys.W)) pos += forward * speed * Window.deltaTime;
        if(Input.KeyHelt(Keys.S)) pos -= forward * speed * Window.deltaTime;
        if(Input.KeyHelt(Keys.D)) pos += right * speed * Window.deltaTime;
        if(Input.KeyHelt(Keys.A)) pos -= right * speed * Window.deltaTime;
    }
}