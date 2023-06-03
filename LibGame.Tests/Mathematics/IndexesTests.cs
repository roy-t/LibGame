using LibGame.Mathematics;

namespace LibGame.Tests.Mathematics;

public static class IndexesTests
{
    [Fact(DisplayName = "`ToOneDimensional` should return a valid one-dimensional index")]
    public static void ToOneDimensional()
    {
        Equal(0, Indexes.ToOneDimensional(0, 0, 100));
        Equal(10, Indexes.ToOneDimensional(10, 0, 100));
        Equal(110, Indexes.ToOneDimensional(10, 1, 100));
    }

    [Fact(DisplayName = "`ToTwoDimensional` should return a valid two-dimensional index")]
    public static void ToTwoDimensional()
    {
        Equal((0, 0), Indexes.ToTwoDimensional(0, 100));
        Equal((99, 0), Indexes.ToTwoDimensional(99, 100));
        Equal((1, 1), Indexes.ToTwoDimensional(101, 100));
    }
}