using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using System.Threading.Tasks;

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

    public static Tri tri = new() {
        pt0 = new(-2, 0, 5),
        pt1 = new(2, 0, 5),
        pt2 = new(0, 3, 5)
    };
    private static void Draw()
    {
        canvas.Clear(Color.Black);

        // RAINBOW DEBUGGER
        //canvas.Clear(Color.FromArgb(255,
        //    (int)(255 * MathF.Max(0, MathF.Sin(Program.secondsPassed))),
        //    (int)(255 * MathF.Max(0, MathF.Cos(Program.secondsPassed))),
        //    (int)(255 * MathF.Max(0, MathF.Sin(Program.secondsPassed - MathF.PI)))));

        for(int x = 0; x < ScreenW; x++)
            for(int y = 0; y < ScreenH; y++)
            {
                Vec3f pt = new(x/ScreenW * 7.5f, y/ScreenH * 7.5f, 5f);
                screen.SetPixel(x, y, PointInsideTriangle(in pt, in tri) ? Color.White : Color.Black);
            }

        // Tps debugger
        canvas.DrawString(Window.tps.ToString("00"), new Font(FontFamily.GenericMonospace, 10), Brushes.White, 3, 3);
    }

    /*
        s = 1 / ( (v1[1] - v2[1]) * (v0[0] - v2[0]) + (v2[0] - v1[0]) * (v0[1] - v2[1]))
        s_a = ((v1[1] - v2[1]) * (pt[0] - v2[0]) + (v2[0] - v1[0]) * (pt[1] - v2[1])) * s
        s_b = ((v2[1] - v0[1]) * (pt[0] - v2[0]) + (v0[0] - v2[0]) * (pt[1] - v2[1])) * s
        s_c = 1 - s_a - s_b
        return 0 <= s_a <= 1 and 0 <= s_b <= 1 and 0 <= s_c <= 1
    */
    private static bool PointInsideTriangle(in Vec3f pt, in Tri tri)
    {
        var (v0, v1, v2) = tri;
        float s = 1f / ((v1.y - v2.y) * (v0.x - v2.x) + (v2.x - v1.x) * (v0.y - v2.y));
        float s_a = ((v1.y - v2.y) * (pt.x - v2.x) + (v2.x - v1.x) * (pt.x - v2.x)) * s;
        float s_b = ((v2.y - v0.y) * (pt.x - v2.x) + (v0.x - v2.x) * (pt.y - v2.y)) * s;
        float s_c = 1 - s_a - s_b;
        return 0 <= s_a && s_a <= 1 && 0 <= s_b  && s_b <= 1 && 0 <= s_c && s_c <= 1;
    }
}