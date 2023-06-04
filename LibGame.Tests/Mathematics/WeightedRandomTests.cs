using LibGame.Mathematics;

namespace LibGame.Tests.Mathematics;

public static class WeightedRandomTests
{
    [Fact(DisplayName = "`Next` should give a weighted random number")]
    public static void Next()
    {
        var random = new WeightedRandom(Random.Shared, new float[] { 1.0f });
        Equal(0, random.Next());
    }

    [Fact(DisplayName = "`Pick` should give a random item from the list")]
    public static void Pick()
    {
        var random = new WeightedRandom(Random.Shared, new float[] { 1.0f });
        var choices = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
        Equal(10, random.Pick(choices));
    }
}
