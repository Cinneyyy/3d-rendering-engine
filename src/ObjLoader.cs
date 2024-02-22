using System.Collections.Generic;
using System.IO;
using System;

namespace src;

public static class ObjLoader
{
    private static readonly char[] newline = Environment.NewLine.ToCharArray();


    public static Mesh LoadObjFile(Stream data)
    {
        using(StreamReader reader = new(data))
            return LoadObjFile(reader.ReadToEnd().Split(newline));
    }
    public static Mesh LoadObjFile(string[] data)
    {
        List<Vec3f> vertices = [];
        List<int[]> faces = [];

        foreach(string ln in data)
        {
            string[] split = ln.Split();

            if(split.Length <= 1)
                continue;

            if(split[0] == "v")
                vertices.Add(new(
                    float.Parse(split[1]),
                    float.Parse(split[2]),
                    float.Parse(split[3])));
            else if(split[0] == "f")
            {
                int[] face = new int[split.Length-1];
                for(int i = 1; i < split.Length; i++)
                    face[i-1] = int.Parse(split[i].Split('/')[0]) - 1;
                faces.Add(face);
            }
            else
                continue;
        }

        List<Tri> tris = [];
        foreach(int[] f in faces)
        {
            switch(f.Length)
            {
                case 3:
                    tris.Add(new(f[0], f[1], f[2])); 
                    break;
                case 4:
                    tris.Add(new(f[0], f[1], f[3]));
                    tris.Add(new(f[1], f[2], f[3]));
                    break;
                default: throw new($"Invalid number of vertices ({f.Length}) on face: [{f.FormatStr(", ")}]");
            }
        }

        return new([.. vertices], [.. tris]);
    }
}