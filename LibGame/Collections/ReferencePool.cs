using System.Collections;

namespace LibGame.Collections;

/// <summary>
/// A pool like collection for reference types, use <see cref="StructPool{T}" /> when T is a struct
/// </summary>
public sealed class ReferencePool<T> : IEnumerable<T>
    where T : class
{
    private T?[] pool;

    private int lowestUnusedSlot = 0;
    private int highestUsedSlot = -1;
    private int count;

    /// <summary>
    /// Create a new pool with the given initial capacity
    /// </summary>    
    public ReferencePool(int initialCapacity = 4)
    {
        this.pool = new T[initialCapacity];
    }

    /// <summary>
    /// The current capacity
    /// </summary>
    public int Capacity => this.pool.Length;

    /// <summary>
    /// Retrieves the element at the given index and returns a reference to it
    /// </summary>        
    /// <exception cref="KeyNotFoundException">If there was no element at the given index</exception>
    public T this[int index]
    {
        get
        {
            if (this.pool[index] != null)
            {
                return this.pool[index]!;
            }

            throw new KeyNotFoundException($"index {index} is not occupied in this pool");
        }
    }

    /// <summary>
    /// Adds the given element
    /// </summary>
    public int Add(T item)
    {
        this.EnsureCapacity(this.count + 1);

        var index = this.lowestUnusedSlot;
        this.lowestUnusedSlot = this.IndexOfFirstUnused(this.lowestUnusedSlot + 1);
        this.highestUsedSlot = Math.Max(this.highestUsedSlot, index);

        this.pool[index] = item;

        this.count += 1;

        return index;
    }

    /// <summary>
    /// Removes the element at the given index. Does nothing if the index is out of range or there was nothing at the given index
    /// </summary>    
    public void Remove(int index)
    {
        if (index < this.pool.Length)
        {
            this.pool[index] = default;

            if (index == this.highestUsedSlot)
            {
                this.highestUsedSlot = this.IndexOfLastUsed(index - 1);
            }

            this.lowestUnusedSlot = Math.Min(this.lowestUnusedSlot, index);

            this.count -= 1;
        }
    }

    /// <summary>
    /// Checks whether an element is present at this[index]
    /// </summary>
    public bool IsOccupied(int index)
    {
        return this.pool[index] != null;
    }

    /// <summary>
    /// Returns the enumerator, allows the user to use foreach(ref var x in this) on this collection
    /// </summary>    
    public IEnumerator<T> GetEnumerator()
    {
        return new ReferencePoolEnumerator<T>(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new ReferencePoolEnumerator<T>(this);
    }

    /// <summary>
    /// Moves items around to fill all holes in the underlying array, note that this changes the indexes
    /// </summary>    
    public void Defrag()
    {
        while (this.lowestUnusedSlot < this.highestUsedSlot)
        {
            this.pool[this.lowestUnusedSlot] = this.pool[this.highestUsedSlot];

            this.lowestUnusedSlot = this.IndexOfFirstUnused(this.lowestUnusedSlot + 1);
            this.highestUsedSlot = this.IndexOfLastUsed(this.highestUsedSlot - 1);
        }
    }

    /// <summary>
    /// Removes all items from the collection
    /// </summary>
    public void Clear()
    {
        this.lowestUnusedSlot = 0;
        this.highestUsedSlot = -1;

        for (var i = 0; i < this.pool.Length; i++)
        {
            this.pool[i] = default;
        }
    }

    /// <summary>
    /// Trims the unused space at the end of the internal array, for maximum space saving
    /// use Defrag first
    /// </summary>    
    public void TrimExcess(int minCapacity = 0)
    {
        if (this.highestUsedSlot < (this.pool.Length - 1))
        {
            var targetCapacity = Math.Max(this.highestUsedSlot + 1, minCapacity);
            Array.Resize(ref this.pool, targetCapacity);
        }
    }

    /// <summary>
    /// Grows the underlying array to at least the given capacity. It is not necessary to call this method
    /// before adding new items. But if its known in advanced how much capacity it is needed calling this method
    /// will prevent unnecessary copying
    /// </summary>    
    public void EnsureCapacity(int capacity)
    {
        if (capacity >= this.pool.Length)
        {
            var newCapacity = Math.Max(capacity, this.pool.Length * 2);

            Array.Resize(ref this.pool, newCapacity);
        }
    }

    private int IndexOfFirstUnused(int minIndex)
    {
        if (minIndex < this.pool.Length)
        {
            for (var i = minIndex; i < this.pool.Length; i++)
            {
                if (this.pool[i] == null)
                {
                    return i;
                }
            }
        }

        return this.pool.Length;
    }

    private int IndexOfLastUsed(int maxIndex)
    {
        if (maxIndex < this.pool.Length)
        {
            for (var i = maxIndex; i >= 0; i--)
            {
                if (this.pool[i] != null)
                {
                    return i;
                }
            }
        }

        return -1;
    }
}
