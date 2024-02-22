namespace src;

public abstract class WorldObject(Vec3f pos, Vec3f rot, Vec3f scale)
{
    public Vec3f pos = pos, rot = rot, scale = scale;
    public abstract Vec3f anchor { get; }


    public WorldObject() : this(Vec3f.zero, Vec3f.zero, Vec3f.one) { }


    public void TranslateX(float x) => pos.x += x;
    public void TranslateY(float y) => pos.y += y;
    public void TranslateZ(float z) => pos.z += z;
    public void Translate(Vec3f xyz) => pos += xyz;
    public void Translate(float x, float y, float z) => Translate(new(x, y, z));

    public void RotateX(float x) => rot.x += x;
    public void RotateY(float y) => rot.y += y;
    public void RotateZ(float z) => rot.z += z;
    public void Rotate(Vec3f xyz) => rot += xyz;
    public void Rotate(float x, float y, float z) => Rotate(new(x, y, z));
}