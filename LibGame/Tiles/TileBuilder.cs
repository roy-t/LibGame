using LibGame.Collections;
using LibGame.Mathematics;

namespace LibGame.Tiles;
public static class TileBuilder
{
    public static Grid<Tile> FromHeightMap(IReadOnlyGrid<byte> heightMap)
    {
        var count = heightMap.Count;
        var columns = heightMap.Columns;
        var rows = heightMap.Rows;
        var tiles = new Grid<Tile>(columns, rows);

        // First tile
        tiles[0, 0] = new Tile(heightMap[0, 0]);

        // First row
        for (var i = 1; i < columns; i++)
        {
            var previous = tiles[i - 1, 0];
            var neighbourhood = heightMap.SliceAtMost(i - 1, 3, 0, 2);
            tiles[i, 0] = FitFirstRow(previous, neighbourhood, heightMap[i, 0]);
        }

        // First column
        for (var i = 1; i < rows; i++)
        {
            var previous = tiles[0, i - 1];
            var neighbourhood = heightMap.SliceAtMost(0, 2, i - 1, 3);
            tiles[0, i] = FitFirstColumn(previous, neighbourhood, heightMap[0, i]);
        }

        // Fill
        for (var i = 0; i < count; i++)
        {
            var (c, r) = Indexes.ToTwoDimensional(i, columns);
            if (c > 0 && r > 0)
            {
                // TODO: why do we need this cast as tiles implements this interface!
                var nwNeighbours = ((IReadOnlyGrid<Tile>)tiles).SliceAtMost(c - 1, 3, r - 1, 3);
                var seNeighbours = heightMap.SliceAtMost(c - 1, 3, r - 1, 3);

                tiles[i] = Fit(nwNeighbours, seNeighbours, heightMap[c, r]);
            }
        }

        return tiles;
    }

    private static Tile FitFirstRow(Tile tile, IReadOnlyGrid<byte> neighbourhood, byte baseHeight)
    {
        var east = neighbourhood.Columns > 2 && neighbourhood.Rows > 0 ? neighbourhood[2, 0] : baseHeight;
        var southEast = neighbourhood.Columns > 2 && neighbourhood.Rows > 1 ? neighbourhood[2, 1] : baseHeight;
        var south = neighbourhood.Columns > 1 && neighbourhood.Rows > 1 ? neighbourhood[1, 1] : baseHeight;
        var southWest = neighbourhood.Columns > 0 && neighbourhood.Rows > 1 ? neighbourhood[0, 1] : baseHeight;

        var hne = Fit(baseHeight, east);
        var hse = Fit(baseHeight, east, southEast, south);
        var hsw = Fit(baseHeight, tile.GetHeight(TileCorner.SE), southWest, south);
        var hnw = Fit(baseHeight, tile.GetHeight(TileCorner.NE));

        return new Tile(hne, hse, hsw, hnw, baseHeight);
    }

    private static Tile FitFirstColumn(Tile tile, IReadOnlyGrid<byte> neighbourhood, byte baseHeight)
    {
        var northEast = neighbourhood.Columns > 1 && neighbourhood.Rows > 0 ? neighbourhood[1, 0] : baseHeight;
        var east = neighbourhood.Columns > 1 && neighbourhood.Rows > 1 ? neighbourhood[1, 1] : baseHeight;
        var southEast = neighbourhood.Columns > 1 && neighbourhood.Rows > 2 ? neighbourhood[1, 2] : baseHeight;
        var south = neighbourhood.Columns > 0 && neighbourhood.Rows > 2 ? neighbourhood[0, 2] : baseHeight;

        var hne = Fit(baseHeight, tile.GetHeight(TileCorner.SE), northEast, east);
        var hse = Fit(baseHeight, east, southEast, south);
        var hsw = Fit(baseHeight, south);
        var hnw = Fit(baseHeight, tile.GetHeight(TileCorner.SW));

        return new Tile(hne, hse, hsw, hnw, baseHeight);
    }

    private static Tile Fit(IReadOnlyGrid<Tile> nwNeighbours, IReadOnlyGrid<byte> seNeighbours, byte baseHeight)
    {
        var baseTile = new Tile(baseHeight);

        var nw = nwNeighbours.Columns > 0 && nwNeighbours.Rows > 0 ? nwNeighbours[0, 0] : baseTile;
        var n = nwNeighbours.Columns > 1 && nwNeighbours.Rows > 0 ? nwNeighbours[1, 0] : baseTile;
        var ne = nwNeighbours.Columns > 2 && nwNeighbours.Rows > 0 ? nwNeighbours[2, 0] : baseTile;
        var w = nwNeighbours.Columns > 0 && nwNeighbours.Rows > 1 ? nwNeighbours[0, 1] : baseTile;

        var heightEast = seNeighbours.Columns > 2 && seNeighbours.Rows > 1 ? seNeighbours[2, 1] : baseHeight;
        var heightSouthWest = seNeighbours.Columns > 0 && seNeighbours.Rows > 2 ? seNeighbours[0, 2] : baseHeight;
        var heightSouth = seNeighbours.Columns > 1 && seNeighbours.Rows > 2 ? seNeighbours[1, 2] : baseHeight;
        var heightSouthEast = seNeighbours.Columns > 2 && seNeighbours.Rows > 2 ? seNeighbours[2, 2] : baseHeight;

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
}
