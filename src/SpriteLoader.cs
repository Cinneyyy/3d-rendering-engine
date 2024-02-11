using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace src;

public static class SpriteLoader
{
    public static class Path
    {
        public const string Resources = "res.";
        public const string App = Resources + "App.";
    }


    public static readonly Dictionary<string, Bitmap> sprites = [];
    public static readonly (string id, string manifest)[] manifestData = [
        ("missing", Path.App + "Missing.png"),
        ("amon", Path.App + "vosuzrp9efj81.jpg"),
        ("bk", Path.App + "bk.PNG")
    ];


    public static Bitmap Get(string id) 
        => sprites.TryGetValue(id, out var bmp) ? bmp : sprites["missing"];

    public static void Load()
    {
        var asm = Assembly.GetExecutingAssembly();

        foreach(var (id, manifest) in manifestData)
            using(Stream? stream = asm.GetManifestResourceStream(manifest))
                if(NotNull(stream, $"Manifest stream for manifest \"{manifest}\" returned null!"))
                    sprites.Add(id, new(stream!));

        //DoColorAdjustments();
    }


    // Uncomment when it needs to be used
    //private static void DoColorAdjustments()
    //{
    //    static void apply(string id, Func<Color, Color> func)
    //    {
    //        Bitmap bmp = Get(id);
    //        int w = bmp.Width, h = bmp.Height;

    //        for (int x = 0; x < w; x++)
    //            for(int y = 0; y < h; y++)
    //                bmp.SetPixel(x, y, func(bmp.GetPixel(x, y)));

    //        sprites[id] = bmp;
    //    }
    //}
}