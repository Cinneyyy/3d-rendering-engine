using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace src;

public static class Renderer
{
    public const int ScreenW = 384;
    public const int ScreenH = 216;
    public const int CenterX = ScreenW/2;
    public const int CenterY = ScreenH/2;

    public static readonly Vec2f screenSize = new(ScreenW, ScreenH);
    public static readonly Vec2f center = new(CenterX - Player.MapX * Player.TileSize / 2, CenterY - Player.MapY * Player.TileSize / 2);
    public static readonly Bitmap screen = new(ScreenW, ScreenH);

    private static readonly Graphics canvas = Graphics.FromImage(screen);

    private static bool drawing = false;
    private static ulong currFrame = 0;
    private static ulong frameSkips = 0;


    public static void Tick(object? sender, ElapsedEventArgs args)
    {
        if((Vec2f)Cursor.Position is var cursor
            && cursor.x >= 0 && cursor.x < Window.curr!.size.x
            && cursor.y >= 0 && cursor.y < Window.curr!.size.y)
        {
            if(!drawing)
            {
                currFrame++;
                PreventFrameHalt(currFrame, 100);

                drawing = true;
                try
                {
                    Draw();
                }
                catch
                {
                    drawing = false;
                    return;
                }

                drawing = false;
                Window.curr!.Refresh();
            }
        }
    }


    public static Vec2f ToCenter(Vec2f pos) => center + pos;
    public static Vec2i ToCenter(Vec2i pos) => center.Round() + pos;


    private static async void PreventFrameHalt(ulong currFrame, int delay)
    {
        await Task.Delay(delay);
        if(Renderer.currFrame == currFrame)
        {
            drawing = false;
            frameSkips++;
            Out($"Frame skip #{frameSkips} ({UtilFuncs.AddSuffix(currFrame)} frame)");
        }
    }

    static Vec3f[] rect = [
        new(-1, -1, -1),
        new(-1, -1,  1),
        new( 1, -1, -1),
        new(-1,  1, -1),
        new(-1,  1,  1),
        new( 1, -1,  1),
        new( 1,  1, -1),
        new( 1,  1,  1)
    ];
    static Vec2i[] edges = [
        new(0, 1),
        new(0, 2),
        new(0, 3),
        new(2, 5),
        new(3, 6),
        new(3, 4),
        new(4, 7),
        new(6, 7),
        new(7, 5),
        new(5, 1),
        new(4, 1),
        new(2, 6)
    ];
    static Vec3f camPos;
    static Vec3f eulerRot;
    static float fl = 10f;
    private static void Draw()
    {
        canvas.Clear(Color.Black);

        // RAINBOW DEBUGGER
        //canvas.Clear(Color.FromArgb(255,
        //    (int)(255 * MathF.Max(0, MathF.Sin(Program.secondsPassed))),
        //    (int)(255 * MathF.Max(0, MathF.Cos(Program.secondsPassed))),
        //    (int)(255 * MathF.Max(0, MathF.Sin(Program.secondsPassed - MathF.PI)))));

        if(Input.KeyHelt(Keys.Left)) eulerRot.x -= Window.deltaTime;
        if(Input.KeyHelt(Keys.Right)) eulerRot.x += Window.deltaTime;
        if(Input.KeyHelt(Keys.Up)) eulerRot.y += Window.deltaTime;
        if(Input.KeyHelt(Keys.Down)) eulerRot.y -= Window.deltaTime;

        if(Input.KeyHelt(Keys.W)) camPos.z += Window.deltaTime;
        if(Input.KeyHelt(Keys.S)) camPos.z -= Window.deltaTime;
        if(Input.KeyHelt(Keys.D)) camPos.x += Window.deltaTime;
        if(Input.KeyHelt(Keys.A)) camPos.x -= Window.deltaTime;

        PointF[] projectedPoints = (from p in rect
                                   select (PointF)(Project3dPoint(p, camPos, eulerRot, fl) * 100f + center))
                                   .ToArray();

        foreach(Vec2i e in edges)
            canvas.DrawLine(new(Brushes.White), projectedPoints[e.x], projectedPoints[e.y]);

        // Tps debugger
        canvas.DrawString(Window.tps.ToString("00"), new Font(FontFamily.GenericMonospace, 10), Brushes.White, 3, 3);
    }

    private static Vec2f Project3dPoint(Vec3f pt, Vec3f cam, Vec3f rot, float fl)
    {
        // Apply rotation to the point and the camera position
        Vec3f rotatedPt = RotateZ(RotateX(RotateY(pt - cam, rot.y), rot.x), rot.z);
        Vec3f rotatedCam = RotateZ(RotateX(RotateY(cam, rot.y), rot.x), rot.z);

        // Translate the rotated point to the origin
        Vec3f translatedPt = rotatedPt - rotatedCam;

        // Perform projection
        return Project(translatedPt, fl);
    }
    //    => Project(RotateZ(RotateX(RotateY(pt - cam, rot.x), rot.y), rot.z) + cam, fl);

    private static Vec3f RotateX(Vec3f pt, float rot)
        => new(pt.x,
               MathF.Cos(rot) * pt.y - MathF.Sin(rot) * pt.z,
               MathF.Sin(rot) * pt.y + MathF.Cos(rot) * pt.z);

    private static Vec3f RotateY(Vec3f pt, float rot)
        => new(MathF.Cos(rot) * pt.x - MathF.Sin(rot) * pt.z,
               pt.y,
               MathF.Sin(rot) * pt.x + MathF.Cos(rot) * pt.z);

    private static Vec3f RotateZ(Vec3f pt, float rot)
        => new(MathF.Cos(rot) * pt.x - MathF.Sin(rot) * pt.y,
               MathF.Sin(rot) * pt.x + MathF.Cos(rot) * pt.y,
               pt.z);

    private static Vec2f Project(Vec3f pt, float fl)
        => new(fl * pt.x / (fl + pt.z),
               fl * pt.y / (fl + pt.z));
}