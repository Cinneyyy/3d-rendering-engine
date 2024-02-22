using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace src;

public static class ResourceLoader
{
    public enum ResType
    {
        Bitmap,
        Mesh
    }

    public static class Path
    {
        public const string RESOURCES = "res.";
        public const string APP = RESOURCES + "App.";
        public const string MODELS = RESOURCES + "Models.";
    }


    public static readonly Dictionary<string, Bitmap> bitmaps = [];
    public static readonly Dictionary<string, Mesh> meshes = [];

    private static readonly (ResType type, string id, string manifest)[] manifestData = [
        (ResType.Bitmap, "missing", Path.APP + "Missing.png"),
        (ResType.Bitmap, "amon", Path.APP + "vosuzrp9efj81.jpg"),
        (ResType.Bitmap, "bk", Path.APP + "bk.PNG"),

        (ResType.Mesh, "monke", Path.MODELS + "monke.obj"),
        (ResType.Mesh, "cube", Path.MODELS + "cube.obj"),
        (ResType.Mesh, "dino", Path.MODELS + "dino.obj"),
        //(ResType.QuadMesh, "torus", Path.Models + "torus.obj"),
        //(ResType.QuadMesh, "th_txt", Path.Models + "text_thin.obj"),
        //(ResType.QuadMesh, "md_txt", Path.Models + "text_medium.obj"),
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
                        case ResType.Mesh: meshes.Add(id, ObjLoader.LoadObjFile(stream!)); break;
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