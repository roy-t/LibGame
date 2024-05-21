using System.Numerics;
using Vortice.Mathematics;

namespace LibGame.Mathematics;
public static class Dragging
{
    /// <summary>
    /// Calculates how much the cursor dragged the object away from its starting position along the Y axis.
    /// Negative values indicate the object was dragged in the -Y direction.
    /// </summary>
    /// <param name="startPosition">The starting position of the object</param>
    /// <param name="cursor">The cursor ray</param>        
    public static float ComputeDragDeltaY(Vector3 startPosition, Ray cursor)
    {
        var normal = Vector3.Normalize(new Vector3(cursor.Direction.X, 0.0f, cursor.Direction.Z));
        var plane = Planes.FromNormalAndPosition(-normal, startPosition);
        var hit = cursor.Intersects(plane);
        if (hit.HasValue)
        {
            var world = cursor.Position + (cursor.Direction * hit.Value);
            return world.Y - startPosition.Y;
        }

        return 0.0f;
    }
}
