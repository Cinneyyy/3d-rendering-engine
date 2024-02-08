using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Timers;
using ST = System.Timers;
using SWF = System.Windows.Forms;

namespace src;

public class Window
{
    public class Canvas : Form
    {
        public Canvas()
            => DoubleBuffered = true;


        public void Center()
            => CenterToScreen();
    }



    public readonly Canvas canvas;
    public readonly Vec2i center;
    public readonly float upscaleFactor, downscaleFactor;

    private const string IconPath = SpriteLoader.Path.App + "appicon.ico";

    private static readonly Icon icon = new(Assembly.GetExecutingAssembly().GetManifestResourceStream(IconPath)!);

    private readonly ST::Timer tickTimer;
    private readonly Vec2i renderSize;
    private readonly int yRenderOffset;

    private DateTime tpscLastTick;
    private int tpscCounter;
    private float tpscTimeCounter;


    public static Window? curr { get; private set; } = null;
    public static bool running { get; private set; } = false;
    public static float tps { get; private set; }
    public static float deltaTime { get; private set; }


    public Vec2i size { get; private set; } = new();
    public Vec2i pos { get; private set; } = new();
    public string title { get; private set; } = string.Empty;
    public event ElapsedEventHandler tick
    {
        add => tickTimer.Elapsed += value;
        remove => tickTimer.Elapsed -= value;
    }


    public Window(string title, float cycleTps)
    {
        this.title = title;
        curr = this;
        running = true;

        canvas = new() {
            Text = title,
            BackColor = Color.Black,
            StartPosition = FormStartPosition.CenterScreen,
            FormBorderStyle = FormBorderStyle.None,
            Size = Screen.PrimaryScreen!.Bounds.Size,
            WindowState = FormWindowState.Maximized,
            Icon = icon
        };

        size = new(canvas.Size.Width, (int)(canvas.Size.Width * ((float)Renderer.ScreenH / Renderer.ScreenW)));
        renderSize = size - new Vec2i(2);
        yRenderOffset = (canvas.Size.Height - size.y) / 2;

        center = size/2;
        upscaleFactor = size.x/(float)Renderer.ScreenW;
        downscaleFactor = Renderer.ScreenW/(float)size.x;

        canvas.Paint += OnPaint!;
        canvas.MouseClick += Input.OnMouseClick!;
        canvas.KeyDown += Input.OnKeyDown!;
        canvas.KeyUp += Input.OnKeyUp!;
        canvas.MouseMove += Input.OnMouseMove!;

        (tickTimer = new() {
            AutoReset = true,
            Enabled = true,
            Interval = 1000d / cycleTps
        }).Start();

        tick += (_, _) => {
            deltaTime = (float)(DateTime.Now - tpscLastTick).TotalSeconds;
            tpscLastTick = DateTime.Now;

            tpscTimeCounter += deltaTime;
            tpscCounter++;
            if(tpscTimeCounter >= 1f)
            {
                tpscTimeCounter %= 1f;
                tps = tpscCounter;
                tpscCounter = 0;
            }

            Cursor.Hide();
        };

        new Thread(() => Application.Run(canvas)).Start();
        new Thread(ConsoleCommands.BeginLoop).Start();
    }


    public void Refresh() =>
        canvas.BeginInvoke((SWF::MethodInvoker)canvas.Refresh);


    private void OnPaint(object sender, PaintEventArgs args)
    {
        args.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
        args.Graphics.SmoothingMode = SmoothingMode.None;
        args.Graphics.PixelOffsetMode = PixelOffsetMode.None;
        args.Graphics.CompositingQuality = CompositingQuality.HighQuality;

        try
        {
            args.Graphics.DrawImage(Renderer.screen, 0, yRenderOffset, renderSize.x, renderSize.y);
        } 
        catch { }
    }


    public static void Exit(int exitCode = 0)
    {
        running = false;
        Environment.Exit(exitCode);
    }
}