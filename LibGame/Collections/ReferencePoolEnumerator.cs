using System.Collections;

namespace LibGame.Collections;

/// <summary>
/// Custom enumerator for <see cref="ReferencePool{T}"/>
/// </summary>
public struct ReferencePoolEnumerator<T> : IEnumerator<T>
    where T : class
{
    private readonly ReferencePool<T> Pool;
    private int index;

    /// <summary>
    /// Creates an enumerator for the given pool
    /// </summary>
    public ReferencePoolEnumerator(ReferencePool<T> pool)
    {
        this.Pool = pool;
        this.Reset();
    }

    /// <inheritdoc/>
    public T Current => this.Pool[this.index]!;

    /// <inheritdoc/>
    object IEnumerator.Current => this.Pool[this.index]!;

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

    /// <inheritdoc/>
    public void Dispose() { }
}
