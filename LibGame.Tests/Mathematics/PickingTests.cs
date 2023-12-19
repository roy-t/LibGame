using System.Drawing;

using LibGame.Mathematics;

namespace LibGame.Tests.Mathematics;

public static class PickingTests
{

    [Fact(DisplayName ="`CalculateCursorRay` should return a ray that intersects with the point in world space")]
    public static void CalculateCursorRay()
    {
        var view = Matrix4x4.CreateLookAt(Vector3.Zero, -Vector3.UnitZ, Vector3.UnitY);
        var proj = Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 2.0f, 1.0f, 0.00001f, 100.0f);

        var wvp = view * proj;

        var point = new Vector3(0, 0, -1);

        var viewport = new Rectangle(0, 0, 100, 100);
        var (position, direction) = Picking.CalculateCursorRay(new Vector2(50, 50), in viewport, in wvp);

        EqualF(point, position + direction);
    }
}
