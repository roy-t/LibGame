using System.Numerics;

namespace LibGame.Mathematics;

/// <summary>
/// Plane utilities
/// </summary>
public static class Planes
{
    /// <summary>
    /// Creates a plane from the plane's normal and a position that lies on the plane
    /// </summary>
    /// <param name="normal">The normal</param>
    /// <param name="position">A position that lies on the plane</param>    
    public static Plane FromNormalAndPosition(Vector3 normal, Vector3 position)
    {
        var distance = -Vector3.Dot(normal, position);
        return new Plane(normal, distance);
    }
}
