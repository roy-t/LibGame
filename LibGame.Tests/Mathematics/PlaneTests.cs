using LibGame.Mathematics;
using Vortice.Mathematics;

namespace LibGame.Tests.Mathematics;
public static class PlaneTests
{
    [Fact(DisplayName = "`FromNormalAndPosition` should generate a plane with the given normal that includes the given position")]
    public static void FromNormalAndPosition()
    {
        var position = Vector3.Zero;
        var normal = Vector3.Normalize(Vector3.One);
        var ray = new Ray(position, normal);

        var plane = Planes.FromNormalAndPosition(normal, Vector3.Zero);
        EqualF(normal, plane.Normal);
        var d = ray.Intersects(plane);
        True(d.HasValue);
        EqualF(d.Value, 0.0f);

        plane = Planes.FromNormalAndPosition(normal, position + (normal * 10.0f));
        EqualF(normal, plane.Normal);
        d = ray.Intersects(plane);
        True(d.HasValue);
        EqualF(10.0f, d.Value);
    }
}
