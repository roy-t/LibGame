using System.Numerics;

namespace LibGame.Geometry;
public static class Quads
{
    public static float GetArea(Vector3 tr, Vector3 br, Vector3 bl, Vector3 tl)
    {
        var areaA = Triangles.GetArea(tr, br, tl);
        var areaB = Triangles.GetArea(br, bl, tl);

        return areaA + areaB;
    }

    public static Vector3 GetNormal(Vector3 tr, Vector3 br, Vector3 bl, Vector3 tl)
    {
        var normalA = Triangles.GetNormal(tr, br, tl);
        var areaA = Triangles.GetArea(tr, br, tl);

        var normalB = Triangles.GetNormal(br, bl, tl);
        var areaB = Triangles.GetArea(br, bl, tl);

        return Vector3.Normalize((normalA * areaA) + (normalB * areaB));
    }
}
