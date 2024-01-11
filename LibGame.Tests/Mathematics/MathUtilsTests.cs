using LibGame.Mathematics;

namespace LibGame.Tests.Mathematics;
public static class MathUtilsTests
{
    [Fact(DisplayName = "`CMod should return a valid index")]
    public static void CMod()
    {
        Equal(0, MathUtils.CMod(0, 5));
        Equal(1, MathUtils.CMod(1, 5));
        Equal(2, MathUtils.CMod(2, 5));
        Equal(3, MathUtils.CMod(3, 5));
        Equal(4, MathUtils.CMod(4, 5));

        Equal(0, MathUtils.CMod(5, 5));
        Equal(0, MathUtils.CMod(15, 5));

        Equal(0, MathUtils.CMod(-0, 5));

        Equal(0, MathUtils.CMod(-5, 5));
        Equal(1, MathUtils.CMod(-4, 5));
        Equal(2, MathUtils.CMod(-3, 5));
        Equal(3, MathUtils.CMod(-2, 5));
        Equal(4, MathUtils.CMod(-1, 5));

        Equal(0, MathUtils.CMod(-15, 5));
    }
}
