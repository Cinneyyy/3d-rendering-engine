using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace src;

#pragma warning disable CA2211
public static class Renderer
{
    public const int ScreenW = 384;
    public const int ScreenH = 216;
    public const int CenterX = ScreenW/2;
    public const int CenterY = ScreenH/2;

    public static readonly Vec2f screenSize = new(ScreenW, ScreenH);
    public static readonly Vec2f screenCenter = new(CenterX, CenterY);
    public static readonly Bitmap screen = new(ScreenW, ScreenH);
    public static readonly List<IRenderableObject> renderObjects = [];
    public static Camera cam = new(new(0f, 0f, -3f), Vec3f.zero, 90f);

    private static readonly Graphics canvas = Graphics.FromImage(screen);

    private static bool drawing = false;
    private static ulong frameCount = 0;
    private static ulong frameSkips = 0;


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


    public static Vec2f ToCenter(Vec2f pos) => screenCenter + pos;
    public static Vec2i ToCenter(Vec2i pos) => screenCenter.Round() + pos;

    public static Vec2f WorldToScreen(Vec2f pt)
        => (pt + Vec2f.one) / 2f * ScreenH + new Vec2f(CenterY, 0);


    private static async void PreventFrameHalt(ulong currFrame, int delay)
    {
        await Task.Delay(delay);
        if(frameCount == currFrame)
        {
            drawing = false;
            frameSkips++;
            //Out($"Frame skip #{frameSkips} ({UtilFuncs.AddSuffix(currFrame)} frame)");
        }
    }

    private static void Draw()
    {
        canvas.Clear(Color.Black);

        foreach(var o in renderObjects)
            o.RenderToScreen(canvas);

        canvas.DrawString(Window.tps.ToString("00"), new Font(FontFamily.GenericMonospace, 10), Brushes.White, 3, 3);
    }
}