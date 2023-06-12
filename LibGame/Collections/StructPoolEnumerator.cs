namespace LibGame.Collections;

/// <summary>
/// Custom enumerator for <see cref="StructPool{T}"/>
/// </summary>
public struct StructPoolEnumerator<T> : IStructEnumerator<T>
where T : struct
{
    private readonly StructPool<T> Pool;
    private int index;

    /// <summary>
    /// Creates an enumerator for the given pool
    /// </summary>
    public StructPoolEnumerator(StructPool<T> pool)
    {
        this.Pool = pool;
        this.Reset();
    }

    /// <inheritdoc/>
    public readonly ref T Current => ref this.Pool[this.index];

    /// <inheritdoc/>
    public bool MoveNext()
    {
        while (this.index < this.Pool.Capacity - 1)
        {
            this.index++;
            if (this.Pool.IsOccupied(this.index))
            {
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc/>
    public void Reset()
    {
        this.index = -1;
    }
}
