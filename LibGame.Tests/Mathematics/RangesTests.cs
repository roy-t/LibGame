using LibGame.Mathematics;

namespace LibGame.Tests.Mathematics;
public static class RangesTests
{
    [Fact(DisplayName = "`Map` should remap a value to the given range")]
    public static void Map()
    {
        Equal(10.0f, Ranges.Map(1.0f, (0.0f, 1.0f), (1.0f, 10.0f)));

        Equal(new Vector2(10.0f, 10.0f), Ranges.Map(new Vector2(1.0f, 1.0f), (0.0f, 1.0f), (1.0f, 10.0f)));



        Equal(new Vector2(10.0f, 20.0f), Ranges.Map(new Vector2(1.0f, 1.0f), (0.0f, 1.0f), (1.0f, 10.0f), (0.5f, 1.0f), (5.0f, 20.0f)));
    }
}
