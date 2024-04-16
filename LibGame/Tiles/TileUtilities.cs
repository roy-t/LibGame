using System.Numerics;

namespace LibGame.Tiles;

public readonly record struct Neighbours<T>(T NW, T N, T NE, T W, T E, T SW, T S, T SE)
    where T : struct
{ }


public static class TileUtilities
{
    /// <summary>
    /// Return the two corners connect to the given side. For example, The north side will give the NE and NW corners.
    /// </summary>
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

    /// <summary>
    /// Returns the opposite side of the given side. For example, north will return south.
    /// </summary>
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

