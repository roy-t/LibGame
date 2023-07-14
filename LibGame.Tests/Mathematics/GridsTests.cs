using LibGame.Mathematics;

namespace LibGame.Tests.Mathematics;
public static class GridsTests
{
    [Fact(DisplayName = "`PickCell` should return the grid cell that corresponds to the given world coordinates")]
    public static void PickCell()
    {
        // Centered around zero

        var (x, y, z) = Grids.PickCell(9, 9, 9, Vector3.One, Vector3.Zero, new Vector3(-4.49f, -4.49f, -4.49f));
        Equal(0, x);
        Equal(0, y);
        Equal(0, z);

        (x, y, z) = Grids.PickCell(9, 9, 9, Vector3.One, Vector3.Zero, new Vector3(4.49f, 4.49f, 4.49f));
        Equal(8, x);
        Equal(8, y);
        Equal(8, z);

        (x, y, z) = Grids.PickCell(9, 9, 9, Vector3.One, new Vector3(0, 0, 0), new Vector3(-0.49f, -0.49f, -0.49f));
        Equal(4, x);
        Equal(4, y);
        Equal(4, z);

        (x, y, z) = Grids.PickCell(9, 9, 9, Vector3.One, new Vector3(0, 0, 0), new Vector3(0.49f, 0.49f, 0.49f));
        Equal(4, x);
        Equal(4, y);
        Equal(4, z);

        // Offset
        (x, y, z) = Grids.PickCell(9, 9, 9, Vector3.One, new Vector3(4.5f, 4.5f, 4.5f), new Vector3(0.5f, 0.5f, 0.5f));
        Equal(0, x);
        Equal(0, y);
        Equal(0, z);

        (x, y, z) = Grids.PickCell(9, 9, 9, Vector3.One, new Vector3(4.5f, 4.5f, 4.5f), new Vector3(8.5f, 8.5f, 8.5f));
        Equal(8, x);
        Equal(8, y);
        Equal(8, z);

        (x, y, z) = Grids.PickCell(9, 9, 9, Vector3.One, new Vector3(4.5f, 4.5f, 4.5f), new Vector3(4.5f, 4.5f, 4.5f));
        Equal(4, x);
        Equal(4, y);
        Equal(4, z);
    }
}
