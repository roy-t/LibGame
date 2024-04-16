namespace LibGame.Tiles;

/// <summary>
/// Enum to indentify a single side of a square tile
/// </summary>
public enum TileSide : byte
{
    North = 0,
    East = 1,
    South = 2,
    West = 3,
}

/// <summary>
/// Enum to indentify a single corner of a square tile
/// </summary>
public enum TileCorner : byte
{
    NE = 0,
    SE = 1,
    SW = 2,
    NW = 3
}

/// <summary>
/// Enum to indentify the type of a corner. Corners can be at most one unit higher or lower than the tile itself.
/// </summary>
public enum CornerType : byte
{
    Level = 0,
    Raised = 1,
    Lowered = 2,
    Mask = 3
}

/// <summary>
/// A single tile that can be used to create tile based terrains, such as in games like Roller Coaster Tycoon,
/// Transport Tycoon, Sim City 2000, etc... A tile has a height. Each corner of the tile can be at most one unit higher or
/// lower than that height. The data is packed into only two bytes, reducing the ammount of memory needed to represent a
/// larger terrains.
/// </summary>
public readonly struct Tile
{
    private const byte MaskNE = 0b_00_00_00_11;
    private const byte MaskSE = 0b_00_00_11_00;
    private const byte MaskSW = 0b_00_11_00_00;
    private const byte MaskNW = 0b_11_00_00_00;

    /// <summary>
    /// The height of the tile
    /// </summary>
    public readonly byte Height;

    /// <summary>
    /// Packed information about each corner type, use the utility methods on this class to decode this information
    /// </summary>
    public readonly byte Corners;

    public Tile(CornerType ne, CornerType se, CornerType sw, CornerType nw, byte height)
    {
        this.Height = height;

        byte corners = 0b_00_00_00_00;
        corners |= (byte)((int)ne << 0);
        corners |= (byte)((int)se << 2);
        corners |= (byte)((int)sw << 4);
        corners |= (byte)((int)nw << 6);

        this.Corners = corners;
    }

    public Tile(byte height)
        : this(CornerType.Level, CornerType.Level, CornerType.Level, CornerType.Level, height) { }

    /// <summary>
    /// Get the type of a single corner
    /// </summary>
    public CornerType GetCorner(TileCorner corner)
    {
        var ic = (int)corner;
        var mask = (byte)(MaskNE << ic * 2);
        return (CornerType)((this.Corners & mask) >> ic * 2);
    }

    /// <summary>
    /// Get the type of all corners
    /// </summary>
    public (CornerType NE, CornerType SE, CornerType SW, CornerType NW) GetAllCorners()
    {
        var ne = (CornerType)((this.Corners & MaskNE) >> 0);
        var se = (CornerType)((this.Corners & MaskSE) >> 2);
        var sw = (CornerType)((this.Corners & MaskSW) >> 4);
        var nw = (CornerType)((this.Corners & MaskNW) >> 6);

        return (ne, se, sw, nw);
    }

    /// <summary>
    /// Get the relative offset (-1, 0, or 1) of this corner from the base height of the tile.
    /// Use <seealso cref="GetHeight(TileCorner)"/> to get the absolute height.
    /// </summary>
    public sbyte GetHeightOffset(TileCorner corner)
    {
        return GetCornerTypeOffset(this.GetCorner(corner));
    }

    /// <summary>
    /// Get the relative offset (-1, 0, or 1) of all corners from the base height of the tile.
    /// Use <seealso cref="GetHeights()"/> to get the absolute heights.
    /// </summary>
    public (sbyte ne, sbyte se, sbyte sw, sbyte nw) GetHeightOffsets()
    {
        var (ne, se, sw, nw) = this.GetAllCorners();
        return
        (
            GetCornerTypeOffset(ne),
            GetCornerTypeOffset(se),
            GetCornerTypeOffset(sw),
            GetCornerTypeOffset(nw)
        );
    }

    /// <summary>
    /// Get the absolute height of this corner, which is the offset of this corner, plus the base height of this tile.
    /// </summary>
    public byte GetHeight(TileCorner corner)
    {
        return (byte)(this.Height + this.GetHeightOffset(corner));
    }

    /// <summary>
    /// Get the absolute height of all corners, which is the offset of each corner, plus the base height of this tile.
    /// </summary>
    public (byte ne, byte se, byte sw, byte nw) GetHeights()
    {
        var (ne, se, sw, nw) = this.GetHeightOffsets();
        return
        (
            (byte)(ne + this.Height),
            (byte)(se + this.Height),
            (byte)(sw + this.Height),
            (byte)(nw + this.Height)
        );
    }

    /// <summary>
    /// Indicates if the tile is level (the offset of each corner is 0).
    /// </summary>
    public bool IsLevel()
    {
        return this.Corners == 0;
    }

    public override string ToString()
    {
        var (ne, se, sw, nw) = this.GetHeightOffsets();
        return $"{this.Height} [{ne:+#;-#;0}, {se:+#;-#;0}, {sw:+#;-#;0}, {nw:+#;-#;0}]";
    }

    /// <summary>
    /// Gives the relative offset (-1, 0, or 1) of the given corner type
    /// </summary>
    public static sbyte GetCornerTypeOffset(CornerType corner)
    {
        return corner switch
        {
            CornerType.Level => 0,
            CornerType.Raised => 1,
            CornerType.Lowered => -1,
            _ => throw new ArgumentOutOfRangeException(nameof(corner)),
        };
    }
}
