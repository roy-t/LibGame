using LibGame.Geometry;

namespace LibGame.Tests.Geometry;
public static class LinesTests
{
    [Fact(DisplayName = "`GetNormalFromLineSegment should return the normal of a line segment")]
    public static void GetNormalFromLineSegment()
    {
        Equal(Vector2.Normalize(new Vector2(-1, 1)), Lines.GetNormalFromLineSegement(Vector2.Zero, Vector2.One));
    }
}
