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
    private const string ICON_PATH = ResourceLoader.Path.APP + "appicon.ico";

    private static readonly Icon icon = new(Assembly.GetExecutingAssembly().GetManifestResourceStream(ICON_PATH)!);

    public readonly Canvas canvas;
    public readonly Vec2i center;
    public readonly float upscaleFactor, downscaleFactor;

    private readonly ST::Timer tickTimer;
    private readonly Vec2i renderSize;
    private readonly int yRenderOffset;
    private DateTime tpscLastTick;
    private int tpscCounter;
    private float tpscTimeCounter;
    private float _targetTps;
    private float _targetInterval;


    public static Window? curr { get; private set; } = null;
    public static bool running { get; private set; } = false;
    public static float tps { get; private set; }
    public static float deltaTime { get; private set; }
    public static int ticksPassed { get; private set; }


    public Vec2i size { get; private set; } = new();
    public Vec2i pos { get; private set; } = new();
    public string title { get; private set; } = string.Empty;
    public event ElapsedEventHandler tick
    {
        add => tickTimer.Elapsed += value;
        remove => tickTimer.Elapsed -= value;
    }
    public event Action<float> update
    {
        add => tick += (_, _) => value(deltaTime);
        remove => tick -= (_, _) => value(deltaTime);
    }
    public float targeTps
    {
        get => _targetTps;
        set => _targetInterval = (float)(tickTimer.Interval = 1000d / (_targetTps = value));
    }
    public float targetInterval
    {
        get => _targetInterval;
        set => _targetTps = 1000f / (float)(tickTimer.Interval = (double)(_targetInterval = value));
    }


    public Window(string title, float targetTps)
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

        size = new(canvas.Size.Width, (int)(canvas.Size.Width * ((float)Renderer.SCREEN_H / Renderer.SCREEN_W)));
        renderSize = size - new Vec2i(2);
        yRenderOffset = (canvas.Size.Height - size.y) / 2;

        center = size/2;
        upscaleFactor = size.x/(float)Renderer.SCREEN_W;
        downscaleFactor = Renderer.SCREEN_W/(float)size.x;

        canvas.Paint += OnPaint!;
        canvas.MouseClick += Input.OnMouseClick!;
        canvas.KeyDown += Input.OnKeyDown!;
        canvas.KeyUp += Input.OnKeyUp!;
        canvas.MouseMove += Input.OnMouseMove!;

        (tickTimer = new() {
            AutoReset = true,
            Enabled = true,
            Interval = 1000d / (_targetTps = targetTps)
        }).Start();

        _targetInterval = 1000f / targetTps;

        tick += (_, _) => {
            ticksPassed++;

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

        args.Graphics.DrawImage(Renderer.screen, 0, yRenderOffset, renderSize.x, renderSize.y);
    }


    public static void Exit(int exitCode = 0)
    {
        running = false;
        Environment.Exit(exitCode);
    }
}