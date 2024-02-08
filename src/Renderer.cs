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

    private static void Draw()
    {
        canvas.Clear(Color.Black);

        // RAINBOW DEBUGGER
        //canvas.Clear(Color.FromArgb(255,
        //    (int)(255 * MathF.Max(0, MathF.Sin(Program.secondsPassed))),
        //    (int)(255 * MathF.Max(0, MathF.Cos(Program.secondsPassed))),
        //    (int)(255 * MathF.Max(0, MathF.Sin(Program.secondsPassed - MathF.PI)))));



        // Tps debugger
        canvas.DrawString(Window.tps.ToString("00"), new Font(FontFamily.GenericMonospace, 10), Brushes.White, 3, 3);
    }
}