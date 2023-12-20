using System.Drawing;
using System.Numerics;

namespace LibGame.Mathematics;
public static class Picking
{
    /// <summary>
    /// Calculates a world space ray starting at the camera's position and pointing in the direction of the cursor
    /// </summary>
    /// <param name="cursor">Offset of the cursor from the top left corner of the window, in pixels</param>
    /// <param name="viewport">Viewport</param>
    /// <param name="worldViewProjection">view projection of the camera, you can add an inverse translation matrix to assist in picking objects</param>
    /// <returns></returns>
    public static (Vector3 position, Vector3 direction) CalculateCursorRay(Vector2 cursor, in Rectangle viewport, in Matrix4x4 worldViewProjection)
    {
        var nearSource = new Vector3(cursor, 0.0f);
        var farSource = new Vector3(cursor, 1.0f);

        var nearPoint = Unproject(in viewport, 0.0f, 1.0f, nearSource, in worldViewProjection);
        var farPoint = Unproject(in viewport, 0.0f, 1.0f, farSource, in worldViewProjection);

        var direction = Vector3.Normalize(farPoint - nearPoint);
        return (nearPoint, direction);
    }

    /// <summary>
    /// Unprojects a vector from screen space into model space.
    /// The source point is transformed from screen space to view space by the inverse of the projection matrix,
    /// then from view space to world space by the inverse of the view matrix, and
    /// finally from world space to model space by the inverse of the world matrix.
    /// Note source.Z must be less than or equal to MaxDepth.
    /// </summary>
    public static Vector3 Unproject(in Rectangle viewport, float minDepth, float maxDepth, Vector3 source, in Matrix4x4 worldViewProjection)
    {
        Matrix4x4.Invert(worldViewProjection, out var matrix);

        source.X = Ranges.Map(source.X, (viewport.X, viewport.X + viewport.Width), (-1.0f, 1.0f));
        source.Y = -Ranges.Map(source.Y, (viewport.Y, viewport.Y + viewport.Height), (-1.0f, 1.0f));
        source.Z = Ranges.Map(source.Z, (minDepth, maxDepth), (-1.0f, 1.0f));

        var vector = Vector3.Transform(source, matrix);
        var a = (source.X * matrix.M14) + (source.Y * matrix.M24) + (source.Z * matrix.M34) + matrix.M44;
        if (!WithinEpsilon(a, 1f))
        {
            vector.X /= a;
            vector.Y /= a;
            vector.Z /= a;
        }
        return vector;
    }

    private static bool WithinEpsilon(float a, float b)
    {
        var num = a - b;
        return (-1.401298E-45f <= num) && (num <= float.Epsilon);
    }
}
