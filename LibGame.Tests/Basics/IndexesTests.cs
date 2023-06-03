using LibGame.Basics;

namespace LibGame.Tests.Basics;

public class IndexesTests
{
    [Fact(DisplayName = "`ToOneDimensional` should return a valid one dimensional index")]
    public void ToOneDimensional_ValidIndexes_ValidIndex()
    {
        Equal(0, Indexes.ToOneDimensional(0, 0, 100));
        Equal(10, Indexes.ToOneDimensional(10, 0, 100));
        Equal(110, Indexes.ToOneDimensional(10, 1, 100));
    }
}