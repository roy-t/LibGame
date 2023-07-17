using System.Numerics;

namespace LibGame.Mathematics;
public static class Grids
{
    /// <summary>
    /// Picks the grid cell that corresponds to the given world coordinates by overlaying the grid onto
    /// the world space such that grid cell (0, 0, 0) is the most left, most bottom, most forward cell (-X, -Y, -Z)
    /// of the area covered. If the given world coordinates are outside of the area covered by the grid
    /// the index of the closest grid cell is returned.
    /// </summary>
    /// <param name="dimX">The number of grid cells in the X-dimension</param>
    /// <param name="dimY">The number of grid cells in the Y-dimension</param>
    /// <param name="dimZ">The number of grid cells in the Z-dimension</param>
    /// <param name="cellSize">The size of each grid cell</param>
    /// <param name="center">The world coordinates of the center of the grid</param>
    /// <param name="world">The world coordinates to find the corresponding grid cell for</param>
    /// <returns>The grid cell containing, or closest to, the given world coordinates</returns>
    public static (int X, int Y, int Z) PickCell(int dimX, int dimY, int dimZ, Vector3 cellSize, Vector3 center, Vector3 world)
    {
        var offset = (new Vector3(dimX, dimY, dimZ) *  cellSize * 0.5f) - center;
        world += offset;
        var grid = world / cellSize;

        var x = (int)Math.Clamp(grid.X, 0, dimX - 1);
        var y = (int)Math.Clamp(grid.Y, 0, dimY - 1);
        var z = (int)Math.Clamp(grid.Z, 0, dimZ - 1);

        return (x, y, z);
    }


    /// <summary>
    /// Gives the world boundaries of the given cell by overlaying the grid onto
    /// the world space such that grid cell (0, 0, 0) is the most left, most bottom, most forward cell (-X, -Y, -Z)
    /// of the area covered.
    /// </summary>
    /// <param name="dimX">The number of grid cells in the X-dimension</param>
    /// <param name="dimY">The number of grid cells in the Y-dimension</param>
    /// <param name="dimZ">The number of grid cells in the Z-dimension</param>
    /// <param name="cellSize">The size of each grid cell</param>
    /// <param name="center">The world coordinates of the center of the grid</param>
    /// <param name="x">The cell X index</param>
    /// <param name="y">The cell Y index</param>
    /// <param name="z">The cell Z index</param>
    /// <returns>The world boundaries of the cell</returns>
    public static (Vector3 Min, Vector3 Max) GetCellBounds(int dimX, int dimY, int dimZ, Vector3 cellSize, Vector3 center, int x, int y, int z)
    {
        var offset = (new Vector3(dimX, dimY, dimZ) * cellSize * 0.5f) - center;

        var minIndex = new Vector3(x, y, z);
        var min = (minIndex * cellSize) - offset;

        var maxIndex = minIndex + Vector3.One;
        var max = (maxIndex * cellSize) - offset;

        return (min, max);
    }
}
