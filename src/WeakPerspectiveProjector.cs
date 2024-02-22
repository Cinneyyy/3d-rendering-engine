namespace src;

public static class WeakPerspectiveProjector
{
    public static Vec3f RotatePtAroundSelf(Vec3f pt, WorldObject obj, Camera cam)
        => obj.rotMatrix.MultiplyWithVec3f(pt - obj.anchor) - cam.pos;

    public static Vec3f RotatePtForCam(Vec3f pt, Camera cam)
        => cam.rotMatrix.MultiplyWithVec3f(pt);

    public static Vec3f Project(Vec3f pt, Camera cam)
        => cam.projectionMatrix.MultiplyWithVec3f(pt);
}