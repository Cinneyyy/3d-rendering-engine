using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace src;

#pragma warning disable CA2211, IDE0052
public static class Renderer
{
    public const int SCREEN_W = 384;
    public const int SCREEN_H = 216;
    public const int CENTER_X = SCREEN_W/2;
    public const int CENTER_Y = SCREEN_H/2;
    public const int LAYER_COUNT = 8;
    public const float RATIO_HW = (float)SCREEN_H / SCREEN_W ;
    public const float RATIO_WH = (float)SCREEN_W / SCREEN_H ;

    private const float FRAME_HALT_LENIENCY = 2.5f;

    public static readonly Vec2f screenSize = new(SCREEN_W, SCREEN_H);
    public static readonly Vec2f screenCenter = new(CENTER_X, CENTER_Y);
    public static readonly Bitmap screen = new(SCREEN_W, SCREEN_H);
    public static readonly List<IRenderableObject>[] renderObjects = new List<IRenderableObject>[LAYER_COUNT];
    public static Camera cam = new(new(0f, 0f, -3f), Vec3f.zero, 90f, .1f, 100f);

    private static readonly Graphics canvas = Graphics.FromImage(screen);
    private static bool drawing = false;


    public static ulong frameCount { get; private set; } = 0;
    public static ulong frameSkips { get; private set; } = 0;


    static Renderer()
        => cam.CalculateProjectionMatrix();


    public static void Tick(object? sender, ElapsedEventArgs args)
    {
        if((Vec2f)Cursor.Position is var cursor
            && cursor.x >= 0 && cursor.x < Window.curr!.size.x
            && cursor.y >= 0 && cursor.y < Window.curr!.size.y)
        {
            if(!drawing)
            {
                frameCount++;
                PreventFrameHalt(frameCount, (int)(FRAME_HALT_LENIENCY * Window.curr!.targetInterval));

                drawing = true;
                try
                {
                    Draw();
                }
                catch(System.Exception e)
                {
                    drawing = false;
                    Out($"Error at frame #{frameCount}: {e}");
                    return;
                }

                drawing = false;
                Window.curr!.Refresh();
            }
        }
    }


    public static Vec2f OffsetToCenter(Vec2f pos) => screenCenter + pos;
    public static Vec2i OffsetToCenter(Vec2i pos) => screenCenter.Round() + pos;

    public static Vec2f WorldToScreen(Vec2f pt, out bool oob)
    {
        oob = MathF.Abs(pt.x) > 1f || MathF.Abs(pt.y) > 1f;

        pt = (pt + Vec2f.one) / 2f;
        pt *= screenSize;
        pt.y = SCREEN_H - pt.y;
        return pt;
    }

    public static void RegisterRenderer(IRenderableObject renderableObject, int layer)
    {
        Out($"Registered new renderer");
        if(renderObjects[layer] == null) renderObjects[layer] = [];
        renderObjects[layer].Add(renderableObject);
    }
    public static void UnregisterRenderer(IRenderableObject renderableObject, int layer) => renderObjects[layer].Remove(renderableObject);


    private static async void PreventFrameHalt(ulong currFrame, int delay)
    {
        await Task.Delay(delay);
        if(frameCount == currFrame)
        {
            drawing = false;
            frameSkips++;
            Out($"Frame skip #{frameSkips} ({UtilFuncs.AddSuffix(currFrame)} frame)");
        }
    }

    private static void Draw()
    {
        canvas.Clear(Color.Black);

        foreach(var oList in renderObjects)
            if(oList != null)
                foreach(var o in oList)
                    o.RenderToScreen(canvas, cam);

        canvas.DrawString(Window.tps.ToString("00"), new Font(FontFamily.GenericMonospace, 10), Brushes.White, 3, 3);
    }
}