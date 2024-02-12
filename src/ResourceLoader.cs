using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace src;

public static class ResourceLoader
{
    public enum ResType
    {
        Bitmap,
        EdgeMesh,
        EdgeMeshAllCons
    }

    public static class Path
    {
        public const string Resources = "res.";
        public const string App = Resources + "App.";
        public const string Models = Resources + "Models.";
    }


    public static readonly Dictionary<string, Bitmap> bitmaps = [];
    public static readonly Dictionary<string, EdgeMesh> edgeMeshes = [];

    private static readonly (ResType type, string id, string manifest)[] manifestData = [
        (ResType.Bitmap, "missing", Path.App + "Missing.png"),
        (ResType.Bitmap, "amon", Path.App + "vosuzrp9efj81.jpg"),
        (ResType.Bitmap, "bk", Path.App + "bk.PNG"),

        (ResType.EdgeMesh, "monke", Path.Models + "monke.obj"),
        (ResType.EdgeMesh, "cube", Path.Models + "cube.obj"),
        (ResType.EdgeMesh, "torus", Path.Models + "torus.obj"),
        (ResType.EdgeMesh, "th_txt", Path.Models + "text_thin.obj"),
        (ResType.EdgeMesh, "md_txt", Path.Models + "text_medium.obj"),
    ];


    static ResourceLoader()
        => Load();


    public static void Load()
    {
        var asm = Assembly.GetExecutingAssembly();

        foreach(var (type, id, manifest) in manifestData)
            using(Stream? stream = asm.GetManifestResourceStream(manifest))
                if(NotNull(stream, $"Manifest stream for manifest \"{manifest}\" returned null!"))
                {
                    switch(type)
                    {
                        case ResType.Bitmap: bitmaps.Add(id, new(stream!)); break;
                        case ResType.EdgeMesh: edgeMeshes.Add(id, ObjLoader.LoadToEdgeMesh(stream!, true)); break;
                        case ResType.EdgeMeshAllCons: edgeMeshes.Add(id, ObjLoader.LoadToEdgeMesh(stream!, false)); break;
                    }
                }

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