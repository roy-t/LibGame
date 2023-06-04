using System.Numerics;

namespace LibGame.Geometry;

/// <summary>
/// Utility functions for quads
/// </summary>
public static class Quads
{
    /// <summary>
    /// Returns the area of the quad
    /// </summary>    
    public static float GetArea(Vector3 tr, Vector3 br, Vector3 bl, Vector3 tl)
    {
        var areaA = Triangles.GetArea(tr, br, tl);
        var areaB = Triangles.GetArea(br, bl, tl);

        return areaA + areaB;
    }

    /// <summary>
    /// Returns the normal of the quad, if you are looking at the quad and see the vertices in clockwise order, the normal will be pointing towards you
    /// </summary>    
    public static Vector3 GetNormal(Vector3 tr, Vector3 br, Vector3 bl, Vector3 tl)
    {
        var normalA = Triangles.GetNormal(tr, br, tl);
        var areaA = Triangles.GetArea(tr, br, tl);

        var normalB = Triangles.GetNormal(br, bl, tl);
        var areaB = Triangles.GetArea(br, bl, tl);

        return Vector3.Normalize((normalA * areaA) + (normalB * areaB));
    }
}
