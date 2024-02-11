using System.Collections;
using System.Collections.Generic;

namespace src;

public struct Tri(Vec3f pt0, Vec3f pt1, Vec3f pt2)
{
    public Vec3f pt0 = pt0, pt1 = pt1, pt2 = pt2;

    public Tri() : this(Vec3f.zero, Vec3f.zero, Vec3f.zero) { }
    public Tri(Vec3f pt1, Vec3f pt2, Vec3f pt3, Vec3f pos) : this(pt1 + pos, pt2 + pos, pt3 + pos) { }


    public readonly void Deconstruct(out Vec3f pt0, out Vec3f pt1, out Vec3f pt2)
    {
        pt0 = this.pt0;
        pt1 = this.pt1;
        pt2 = this.pt2;
    }
}