using LibGame.Mathematics;

namespace LibGame.Tests.Mathematics;
public static class IntersectionsTests
{
    [Fact(DisplayName = "`RayTriangle` should determine where the ray intersects the triangle")]
    public static void RayShouldIntersectTriangle()
    {
        var position = new Vector3(0, 1, 0);
        var direction = new Vector3(0, -1, 0);

        var n = new Vector3(0, 0, -1);
        var e = new Vector3(1, 0, 1);
        var w = new Vector3(-1, 0, 1);
        var f = Intersections.RayTriangle(in position, in direction, in n, in e, in w);

        NotNull(f);
        Equal(1.0f, f.Value);


        position = new Vector3(10, 1, 1);
        f = Intersections.RayTriangle(in position, in direction, in n, in e, in w);
        Null(f);
    }
}
