using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Reflection;

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
    private static ulong frameCount = 0;
    private static ulong frameSkips = 0;


    static Renderer()
        => obj = ObjLoader.LoadToEdgeMesh(Assembly.GetExecutingAssembly()!.GetManifestResourceStream("res.Models.monke.obj")!);


    public static void Tick(object? sender, ElapsedEventArgs args)
    {
        if((Vec2f)Cursor.Position is var cursor
            && cursor.x >= 0 && cursor.x < Window.curr!.size.x
            && cursor.y >= 0 && cursor.y < Window.curr!.size.y)
        {
            if(!drawing)
            {
                frameCount++;
                PreventFrameHalt(frameCount, 100);

                drawing = true;
                try
                {
                    Draw();
                }
                catch//(Exception e)
                {
                    drawing = false;
                    //Out($"Error at frame #{frameCount}: {e}");
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
        if(Renderer.frameCount == currFrame)
        {
            drawing = false;
            frameSkips++;
            //Out($"Frame skip #{frameSkips} ({UtilFuncs.AddSuffix(currFrame)} frame)");
        }
    }

    static EdgeMesh obj;
    #region Meshes
    static Mesh cubeVerts = new(new Vec3f[] {
        new(-1, -1, -1), // 0
        new(-1, -1,  1), // 1
        new( 1, -1, -1), // 2
        new(-1,  1, -1), // 3
        new(-1,  1,  1), // 4
        new( 1, -1,  1), // 5
        new( 1,  1, -1), // 6
        new( 1,  1,  1)  // 7
    });
    static QuadMesh cubeQuads = new(cubeVerts, [
        new(0, 3, 6, 2, Brushes.Red),
        new(1, 5, 7, 4, Brushes.Orange),
        new(2, 5, 7, 6, Brushes.Blue),
        new(0, 1, 4, 3, Brushes.Violet),
        new(0, 2, 5, 1, Brushes.Green),
        new(3, 6, 7, 4, Brushes.Cyan)

    ]);
    static EdgeMesh cubeEdges = new(cubeVerts, [
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
            new(2, 6),

            new(0, 7),
            new(2, 4),
            new(1, 6),
            new(3, 5)
    ]);
    #endregion
    public static Vec3f camPos {get;set;} = new(0, 0, -3);
    static Vec3f camRot;
    static float fov = 90f * (MathF.PI/180f);
    static Vec3f objRot = Vec3f.zero;
    private static void Draw()
    {
        canvas.Clear(Color.Black);

        // RAINBOW DEBUGGER
        //canvas.Clear(Color.FromArgb(255,
        //    (int)(255 * MathF.Max(0, MathF.Sin(Program.secondsPassed))),
        //    (int)(255 * MathF.Max(0, MathF.Cos(Program.secondsPassed))),
        //    (int)(255 * MathF.Max(0, MathF.Sin(Program.secondsPassed - MathF.PI)))));

        // Move
        if(Input.KeyHelt(Keys.Left)) camRot.y -= Window.deltaTime;
        if(Input.KeyHelt(Keys.Right)) camRot.y += Window.deltaTime;
        if(Input.KeyHelt(Keys.Up)) camRot.x += Window.deltaTime;
        if(Input.KeyHelt(Keys.Down)) camRot.x -= Window.deltaTime;

        Vec3f move = new();
        if(Input.KeyHelt(Keys.W)) move.z += Window.deltaTime;
        if(Input.KeyHelt(Keys.S)) move.z -= Window.deltaTime;
        if(Input.KeyHelt(Keys.D)) move.x += Window.deltaTime;
        if(Input.KeyHelt(Keys.A)) move.x -= Window.deltaTime;
        if(Input.KeyHelt(Keys.Space)) move.y += Window.deltaTime;
        if(Input.KeyHelt(Keys.ShiftKey)) move.y -= Window.deltaTime;
        camPos += move;

        if(Input.KeyHelt(Keys.I)) fov += Window.deltaTime;
        if(Input.KeyHelt(Keys.U)) fov -= Window.deltaTime;

        objRot += new Vec3f(Window.deltaTime);

        // Project
        //cubeEdges.projectionBuffer =
        //    (from v in cubeEdges.vertices
        //     let proj = (PointF)WorldToScreen(Project3dPoint(v, camPos, camRot, fov))
        //     select proj with { Y = ScreenH - proj.Y })
        //    .ToArray();
        //cubeEdges.DrawToScreen(canvas);

        // Project
        obj.projectionBuffer =
            (from v in obj.GetVertices()
             let proj = (PointF)WorldToScreen(Project3dPoint(v, objRot, obj.anchor, obj.offset, camPos, camRot, fov))
             select proj with { Y = ScreenH - proj.Y })
            .ToArray();
        obj.DrawToScreen(canvas);

        // Project
        //cubeQuads.projectionBuffer =
        //    (from v in cubeQuads.vertices
        //     let proj = (PointF)WorldToScreen(Project3dPoint(v, camPos, camRot, fov))
        //     select proj with { Y = ScreenH - proj.Y })
        //    .ToArray();
        //cubeQuads.DrawToScreen(canvas);

        //canvas.DrawImage(SpriteLoader.Get("amon"), new PointF(50, 50), new(300, 30), new(300, 170), new(50, 150));

        // Boykisser
        //canvas.DrawImage(SpriteLoader.Get("bk"), 50, 50, (MathF.Sin(frameCount/20f) + 1.5f) * 100, (MathF.Cos(frameCount/30f + MathF.PI/2f) + 1.5f) * 100);

        // Tps debugger
        canvas.DrawString(Window.tps.ToString("00"), new Font(FontFamily.GenericMonospace, 10), Brushes.White, 3, 3);
    }

    private static Vec2f Project3dPoint(Vec3f pt, Vec3f ptRot, Vec3f ptRotOrigin, Vec3f objOffset, Vec3f camPos, Vec3f camRot, float fov)
    {
        Vec3f rotated = RotateYXZ(RotateYXZ(pt - ptRotOrigin, ptRot) - camPos + objOffset, camRot);
        Vec3f translated = rotated - new Vec3f(0f, 0f, fov);
        return Project(translated, fov);
    }

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

    private static Vec3f RotateYXZ(Vec3f pt, Vec3f rot)
        => RotateZ(RotateX(RotateY(pt, rot.y), rot.x), rot.z);

    private static Vec2f Project(Vec3f pt, float fov)
        => new(fov * pt.x / (fov + pt.z),
               fov * pt.y / (fov + pt.z));

    private static Vec2f WorldToScreen(Vec2f pt)
        => (pt + Vec2f.one) / 2f * ScreenH + new Vec2f(CenterY, 0);
}