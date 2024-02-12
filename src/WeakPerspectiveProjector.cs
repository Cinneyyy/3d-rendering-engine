using System.Windows.Forms;
using System;

namespace src;

public static class WeakPerspectiveProjector
{
    public static Vec2f Project3dPoint(Vec3f pt, Vec3f ptRot, Vec3f ptRotOrigin, Vec3f objOffset, Vec3f camPos, Vec3f camRot, float fov)
    {
        Vec3f rotated = RotateYXZ(RotateYXZ(pt - ptRotOrigin, ptRot) - camPos + objOffset, camRot);
        Vec3f translated = rotated - new Vec3f(0f, 0f, fov);
        return Project(translated, fov);
    }

    public static Vec3f RotateX(Vec3f pt, float rot)
        => new(pt.x,
               MathF.Cos(rot) * pt.y - MathF.Sin(rot) * pt.z,
               MathF.Sin(rot) * pt.y + MathF.Cos(rot) * pt.z);

    public static Vec3f RotateY(Vec3f pt, float rot)
        => new(MathF.Cos(rot) * pt.x - MathF.Sin(rot) * pt.z,
               pt.y,
               MathF.Sin(rot) * pt.x + MathF.Cos(rot) * pt.z);

    public static Vec3f RotateZ(Vec3f pt, float rot)
        => new(MathF.Cos(rot) * pt.x - MathF.Sin(rot) * pt.y,
               MathF.Sin(rot) * pt.x + MathF.Cos(rot) * pt.y,
               pt.z);

    public static Vec3f RotateYXZ(Vec3f pt, Vec3f rot)
        => RotateZ(RotateX(RotateY(pt, rot.y), rot.x), rot.z);

    public static Vec2f Project(Vec3f pt, float fov)
        => new(fov * pt.x / (fov + pt.z),
               fov * pt.y / (fov + pt.z));
}