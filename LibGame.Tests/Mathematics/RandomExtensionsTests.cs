using LibGame.Mathematics;

namespace LibGame.Tests.Mathematics;
public static class RandomExtensionsTests
{
    [Fact(DisplayName = "`InRange` give a random number in range")]
    public static void InRange()
    {
        var random = new Random();
        const float MIN = 32.3553f;
        const float MAX = 852.421f;

        for (var i = 0; i < 100; i++)
        {
            var value = random.InRange(MIN, MAX);
            Assert.InRange(value, MIN, MAX);
        }        
    }
}
