using LibGame.Tiles;

namespace LibGame.Tests.Tiles;
public static class TileTests
{
    [Fact(DisplayName = "The `Tile` utility methods should return the correct base height and corner information")]
    public static void SmokeTest()
    {
        var tile = new Tile(ne: CornerType.Level, se: CornerType.Raised, sw: CornerType.Lowered, nw: CornerType.Level, height: 128);

        var type = tile.GetCorner(TileCorner.SE);
        Equal(CornerType.Raised, type);

        var (ne, se, sw, nw) = tile.GetAllCorners();

        Equal(CornerType.Level, ne);
        Equal(CornerType.Raised, se);
        Equal(CornerType.Lowered, sw);
        Equal(CornerType.Level, nw);

        var offset = tile.GetHeightOffset(TileCorner.SW);
        Equal(-1, offset);

        var (one, ose, osw, onw) = tile.GetHeightOffsets();

        Equal(0, one);
        Equal(1, ose);
        Equal(-1, osw);
        Equal(0, onw);

        var height = tile.GetHeight(TileCorner.NE);
        Equal(128, height);

        var (hne, hse, hsw, hnw) = tile.GetHeights();
        Equal(128, hne);
        Equal(129, hse);
        Equal(127, hsw);
        Equal(128, hnw);

        False(tile.IsLevel());
        True(new Tile(128).IsLevel());

        Equal(0, Tile.GetCornerTypeOffset(CornerType.Level));
        Equal(1, Tile.GetCornerTypeOffset(CornerType.Raised));
        Equal(-1, Tile.GetCornerTypeOffset(CornerType.Lowered));

        Throws<ArgumentOutOfRangeException>(() => Tile.GetCornerTypeOffset(CornerType.Mask));
    }
}
