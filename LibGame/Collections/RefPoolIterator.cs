namespace LibGame.Collections;

public struct RefPoolIterator<T> : IRefEnumerator<T>
where T : struct
{
    private readonly RefPool<T> Pool;
    private int index;

    public RefPoolIterator(RefPool<T> pool)
    {
        this.Pool = pool;
        this.Reset();
    }

    /// <inheritdoc/>
    public ref T Current => ref this.Pool[this.index];

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
