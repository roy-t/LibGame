using LibGame.Collections;
using LibGame.Mathematics;

namespace LibGame.Tests.Collections;
public static class GridTests
{
    private readonly record struct CR(int Column, int Row);

    [Fact(DisplayName = "Smoke test for `Grid<T>`")]
    public static void SmokeTest()
    {
        var values = new CR[]
        {
            new CR(0, 0), new CR(1, 0), new CR(2, 0),
            new CR(0, 1), new CR(1, 1), new CR(2, 1),
            new CR(0, 2), new CR(1, 2), new CR(2, 2),
            new CR(0, 3), new CR(1, 3), new CR(2, 3),
        };

        var grid = new Grid<CR>(values, 3, 4);

        Equal(3, grid.Columns);
        Equal(4, grid.Rows);
        Equal(12, grid.Count);

        for (var i = 0; i < 12; i++)
        {
            var (c, r) = Indexes.ToTwoDimensional(i, 3);
            var value = new CR(c, r);
            Equal(grid[i], value);
            Equal(grid[c, r], value);
        }

        var slice = grid.Slice(2, 1, 2, 1);
        Equal(1, slice.Columns);
        Equal(1, slice.Rows);

        Equal(new CR(2, 2), slice[0]);

        var safeSlice = grid.SliceAtMost(2, 10, 2, 10);
        Equal(1, safeSlice.Columns);
        Equal(2, safeSlice.Rows);
    }
}
