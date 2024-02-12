using System.Collections.Generic;
using System.IO;
using System;

namespace src;

public static class ObjLoader
{
    private static readonly char[] separator = [' '];


    public static EdgeMesh LoadToEdgeMesh(Stream data, bool makeOnlyNecesseryEdges)
    {
        using(StreamReader reader = new(data))
            return LoadToEdgeMesh(reader.ReadToEnd().Split(Environment.NewLine.ToCharArray()), makeOnlyNecesseryEdges);
    }

    public static EdgeMesh LoadToEdgeMesh(string[] data, bool makeOnlyNecesseryEdges)
    {
        List<Vec3f> vertices = [];
        List<int[]> faces = [];

        foreach(string ln in data)
        {
            string[] split = ln.Split();

            if(split.Length <= 1)
                continue;

            switch(split[0])
            {
                case "v":
                    vertices.Add(new(
                        float.Parse(split[1]),
                        float.Parse(split[2]),
                        float.Parse(split[3])));
                    break;

                case "f":
                {
                    int[] face = new int[split.Length-1];
                    for(int i = 1; i < split.Length; i++)
                        face[i-1] = int.Parse(split[i].Split('/')[0].ToString()) - 1;
                    faces.Add(face);
                    break;
                }
            }
        }

        List<EdgeMesh.Edge> edges = [];
        foreach(int[] f in faces)
        {
            if(makeOnlyNecesseryEdges)
                for(int i = 0; i < f.Length; i++)
                    edges.Add(new EdgeMesh.Edge(f[i], f[(i+1) % f.Length]));
            else
                for(int i = 0; i < f.Length; i++)
                    for(int j = 0; j < f.Length; j++)
                        if(i != j)
                        {
                            EdgeMesh.Edge e = new(f[i], f[j]);
                            if(!edges.Contains(e))
                                edges.Add(e);
                        }
        }

        return new([.. vertices], [.. edges]);
    }
}