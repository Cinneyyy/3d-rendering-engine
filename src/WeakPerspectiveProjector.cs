namespace src;

public static class WeakPerspectiveProjector
{
    public static Vec2f Project3dPoint(Vec3f pt, WorldObject obj, Camera cam)
    {
        Vec3f rotated = obj.rotMatrix.MultiplyWithVec3f(pt - obj.anchor) - cam.pos + obj.pos;
        Vec3f rotatedCam = cam.rotMatrix.MultiplyWithVec3f(rotated);
        Vec3f projected = cam.projectionMatrix.MultiplyWithVec3f(rotatedCam);
        return (Vec2f)projected;
    }
}