using System.Numerics;
using LibGame.Mathematics;

namespace LibGame.Tiles;

public readonly record struct Neighbours<T>(T NW, T N, T NE, T W, T E, T SW, T S, T SE)
    where T : struct
{ }


public static class TileUtilities
{
    // Returns corners for each side from left to right
    public static (TileCorner A, TileCorner B) TileSideToTileCorners(TileSide side)
    {
        return side switch
        {
            TileSide.North => (TileCorner.NW, TileCorner.NE),
            TileSide.East => (TileCorner.NE, TileCorner.SE),
            TileSide.South => (TileCorner.SE, TileCorner.SW),
            TileSide.West => (TileCorner.SW, TileCorner.NW),
            _ => throw new ArgumentOutOfRangeException(nameof(side)),
        };
    }

    public static TileSide GetOppositeSide(TileSide side)
    {
        return side switch
        {
            TileSide.North => TileSide.South,
            TileSide.East => TileSide.West,
            TileSide.South => TileSide.North,
            TileSide.West => TileSide.East,
            _ => throw new ArgumentOutOfRangeException(nameof(side)),
        };
    }

    public static (int x, int y) GetNeighbourIndex(int x, int y, TileSide side)
    {
        return side switch
        {
            TileSide.North => (x + 0, y - 1),
            TileSide.East => (x + 1, y + 0),
            TileSide.South => (x + 0, y + 1),
            TileSide.West => (x - 1, y + 0),
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };
    }



    public static Neighbours<T> GetNeighboursFromGrid<T>(T[] grid, int columns, int rows, int index, T fallback)
        where T : struct
    {
        var nw = GetFromGrid(grid, columns, rows, index, -1, -1, fallback);
        var n = GetFromGrid(grid, columns, rows, index, 0, -1, fallback);
        var ne = GetFromGrid(grid, columns, rows, index, 1, -1, fallback);
        var w = GetFromGrid(grid, columns, rows, index, -1, 0, fallback);
        var e = GetFromGrid(grid, columns, rows, index, 1, 0, fallback);
        var sw = GetFromGrid(grid, columns, rows, index, -1, 1, fallback);
        var s = GetFromGrid(grid, columns, rows, index, 0, 1, fallback);
        var se = GetFromGrid(grid, columns, rows, index, 1, 1, fallback);

        return new Neighbours<T>(nw, n, ne, w, e, sw, s, se);
    }


    public static T GetFromGrid<T>(T[] grid, int columns, int rows, int index, int offsetColumn, int offsetRow, T fallBack)
    {
        var (c, r) = Indexes.ToTwoDimensional(index, columns);
        c += offsetColumn;
        r += offsetRow;

        if (c >= 0 && c < columns && r >= 0 && r < rows)
        {
            var i = Indexes.ToOneDimensional(c, r, columns);
            return grid[i];
        }

        return fallBack;
    }

    public static Tile FitFirstColumn(Tile n, byte heightNorthEast, byte heightEast, byte heightSouth, byte heightSouthEast, byte baseHeight)
    {
        var hne = Fit(baseHeight, n.GetHeight(TileCorner.SE), heightNorthEast, heightEast);
        var hse = Fit(baseHeight, heightEast, heightSouthEast, heightSouth);
        var hsw = Fit(baseHeight, heightSouth);
        var hnw = Fit(baseHeight, n.GetHeight(TileCorner.SW));

        return new Tile(hne, hse, hsw, hnw, baseHeight);
    }

    public static Tile FitFirstRow(Tile w, byte heightEast, byte heightSouthWest, byte heightSouth, byte heightSouthEast, byte baseHeight)
    {
        var hne = Fit(baseHeight, heightEast);
        var hse = Fit(baseHeight, heightEast, heightSouthEast, heightSouth);
        var hsw = Fit(baseHeight, w.GetHeight(TileCorner.SE), heightSouthWest, heightSouth);
        var hnw = Fit(baseHeight, w.GetHeight(TileCorner.NE));

        return new Tile(hne, hse, hsw, hnw, baseHeight);
    }


    public static Tile Fit(Tile nw, Tile n, Tile ne, Tile w, byte heightEast, byte heightSouthWest, byte heightSouth, byte heightSouthEast, byte baseHeight)
    {
        var hne = Fit(baseHeight, n.GetHeight(TileCorner.SE), ne.GetHeight(TileCorner.SW), heightEast);
        var hse = Fit(baseHeight, heightEast, heightSouthEast, heightSouth);
        var hsw = Fit(baseHeight, w.GetHeight(TileCorner.SE), heightSouthWest, heightSouth);
        var hnw = Fit(baseHeight, nw.GetHeight(TileCorner.SE), n.GetHeight(TileCorner.SW), w.GetHeight(TileCorner.NE));

        return new Tile(hne, hse, hsw, hnw, baseHeight);
    }

    private static CornerType Fit(byte baseHeight, params byte[] options)
    {
        var result = baseHeight;
        for (var i = 0; i < options.Length; i++)
        {
            var height = options[i];
            if (IsWithin(height, baseHeight - 1, baseHeight + 1))
            {
                result = height;
                break;
            }
        }

        if (result > baseHeight)
        {
            return CornerType.Raised;
        }

        if (result < baseHeight)
        {
            return CornerType.Lowered;
        }

        return CornerType.Level;
    }

    private static bool IsWithin(int value, int min, int max)
    {
        return value <= max || value >= min;
    }

    public static Vector3 GetCornerPosition(int column, int row, Tile tile, TileCorner c)
    {
        var offset = tile.GetHeight(c);

        return c switch
        {
            TileCorner.NE => new Vector3(column + 1.0f, offset, row + 0.0f),
            TileCorner.SE => new Vector3(column + 1.0f, offset, row + 1.0f),
            TileCorner.SW => new Vector3(column + 0.0f, offset, row + 1.0f),
            TileCorner.NW => new Vector3(column + 0.0f, offset, row + 0.0f),
            _ => throw new IndexOutOfRangeException(),
        }; ;
    }

    public static bool AreSidesAligned(Tile a, Tile b, TileSide tileASide)
    {
        switch (tileASide)
        {
            case TileSide.North:
                return AreCornersAligned(a, TileCorner.NW, b, TileCorner.SW) &&
                       AreCornersAligned(a, TileCorner.NE, b, TileCorner.SE);
            case TileSide.East:
                return AreCornersAligned(a, TileCorner.NE, b, TileCorner.NW) &&
                       AreCornersAligned(a, TileCorner.SE, b, TileCorner.SW);
            case TileSide.South:
                return AreCornersAligned(a, TileCorner.SW, b, TileCorner.NW) &&
                       AreCornersAligned(a, TileCorner.SE, b, TileCorner.NE);
            case TileSide.West:
                return AreCornersAligned(a, TileCorner.NW, b, TileCorner.NE) &&
                       AreCornersAligned(a, TileCorner.SW, b, TileCorner.SE);
            default:
                throw new ArgumentOutOfRangeException(nameof(tileASide));
        }
    }

    public static bool AreCornersAligned(Tile a, TileCorner a0, Tile b, TileCorner b0)
    {
        return a.GetHeight(a0) == b.GetHeight(b0);
    }
}

