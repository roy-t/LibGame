using System.Diagnostics;
using System.Numerics;

namespace LibGame.Geometry;

/// <summary>
/// Utility functions for lines and line segments
/// </summary>
public static class Lines
{
    /// <summary>
    /// Returns the normal of the 2D line segment, if you would walk the line from start to end the normal would be on your left.
    /// </summary>    
    public static Vector2 GetNormalFromLineSegement(Vector2 start, Vector2 end)
    {
        Debug.Assert(start != end, $"{start} == {end}");

        var dx = end.X - start.X;
        var dy = end.Y - start.Y;

        return Vector2.Normalize(new Vector2(-dy, dx));
    }  
}
