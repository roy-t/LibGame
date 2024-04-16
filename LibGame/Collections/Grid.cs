using System.Diagnostics;
using LibGame.Mathematics;
using LibGame.Tiles;

namespace LibGame.Collections;

public static class GridExtensions
{
    public static IReadOnlyGrid<T> SliceAtMost<T>(this IReadOnlyGrid<T> grid, int columnOffset, int columnSpan, int rowOffset, int rowSpan)
    {
        columnSpan = Math.Min(grid.Columns - columnOffset, columnSpan);
        rowSpan = Math.Min(grid.Rows - rowOffset, rowSpan);

        return grid.Slice(columnOffset, columnSpan, rowOffset, rowSpan);
    }

    public static (int column, int row) GetNeighbourIndex<T>(this IReadOnlyGrid<T> grid, int column, int row, TileSide side)
    {
        var (nc, nr) = side switch
        {
            TileSide.North => (column + 0, row - 1),
            TileSide.East => (column + 1, row + 0),
            TileSide.South => (column + 0, row + 1),
            TileSide.West => (column - 1, row + 0),
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };

        if (nc < 0 || nc >= grid.Columns)
        {
            throw new ArgumentOutOfRangeException(nameof(side));
        }

        if (nr < 0 || nr >= grid.Rows)
        {
            throw new ArgumentOutOfRangeException(nameof(side));
        }

        return (nc, nr);
    }
}

public interface IReadOnlyGrid<T>
{
    public T this[int index] { get; }
    public T this[int column, int row] { get; }

    public int Columns { get; }
    public int Rows { get; }
    public int Count { get; }

    public IReadOnlyGrid<T> Slice(int columnOffset, int columnSpan, int rowOffset, int rowSpan);
}

public readonly struct ReadOnlyGridSlice<T> : IReadOnlyGrid<T>
{
    private readonly int ColumnOffset;
    private readonly int ColumnSpan;
    private readonly int RowOffset;
    private readonly int RowSpan;

    private readonly T[] tiles;
    private readonly int stride;

    internal ReadOnlyGridSlice(T[] tiles, int stride, int columnOffset, int columnSpan, int rowOffset, int rowSpan)
    {
        Debug.Assert(stride > 0);
        Debug.Assert(stride <= tiles.Length);

        Debug.Assert(columnOffset >= 0);
        Debug.Assert(columnSpan > 0);

        Debug.Assert(rowOffset >= 0);
        Debug.Assert(rowSpan > 0);



        this.tiles = tiles;
        this.stride = stride;

        this.ColumnOffset = columnOffset;
        this.ColumnSpan = columnSpan;
        this.RowOffset = rowOffset;
        this.RowSpan = rowSpan;
    }

    public T this[int index]
    {
        get
        {
            var (c, r) = Indexes.ToTwoDimensional(index, this.ColumnSpan);
            return this[c, r];
        }
    }

    public T this[int column, int row]
    {
        get
        {
            var c = column + this.ColumnOffset;
            var r = row + this.RowOffset;

            if (c < this.ColumnOffset || c >= this.ColumnOffset + this.ColumnSpan)
            {
                throw new ArgumentOutOfRangeException(nameof(column));
            }

            if (r < this.RowOffset || r >= this.RowOffset + this.RowSpan)
            {
                throw new ArgumentOutOfRangeException(nameof(row));
            }

            return this.tiles[Indexes.ToOneDimensional(c, r, this.stride)];
        }
    }

    public int Columns => this.ColumnSpan;
    public int Rows => this.RowSpan;
    public int Count => this.ColumnSpan * this.RowSpan;

    public IReadOnlyGrid<T> Slice(int columnOffset, int columnSpan, int rowOffset, int rowSpan)
    {
        if (columnSpan > this.ColumnSpan)
        {
            throw new ArgumentOutOfRangeException(nameof(columnSpan));
        }

        if (rowSpan > this.RowSpan)
        {
            throw new ArgumentOutOfRangeException(nameof(rowSpan));
        }

        return new ReadOnlyGridSlice<T>(this.tiles, this.stride,
            this.ColumnOffset + columnOffset,
            columnSpan,
            this.RowOffset + rowOffset,
            rowSpan);
    }
}

public sealed class Grid<T> : IReadOnlyGrid<T>
{
    private readonly T[] Tiles;

    public Grid(int columns, int rows)
        : this(new T[columns * rows], columns, rows) { }

    public Grid(T[] tiles, int columns, int rows)
    {
        this.Tiles = tiles;
        this.Columns = columns;
        this.Rows = rows;
    }

    public int Columns { get; }
    public int Rows { get; }
    public int Count => this.Tiles.Length;

    public T this[int index]
    {
        get => this.Tiles[index];
        set => this.Tiles[index] = value;
    }

    public T this[int column, int row]
    {
        get => this.Tiles[Indexes.ToOneDimensional(column, row, this.Columns)];
        set => this.Tiles[Indexes.ToOneDimensional(column, row, this.Columns)] = value;
    }

    public IReadOnlyGrid<T> AsReadOnly()
    {
        return this;
    }

    public IReadOnlyGrid<T> Slice(int columnOffset, int columnSpan, int rowOffset, int rowSpan)
    {
        if (columnOffset + columnSpan > this.Columns)
        {
            throw new ArgumentOutOfRangeException(nameof(columnSpan));
        }

        if (rowOffset + rowSpan > this.Rows)
        {
            throw new ArgumentOutOfRangeException(nameof(rowSpan));
        }

        return new ReadOnlyGridSlice<T>(this.Tiles, this.Columns, columnOffset, columnSpan, rowOffset, rowSpan);
    }
}
