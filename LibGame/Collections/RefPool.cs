using System.Collections;
using System.ComponentModel;

namespace LibGame.Collections;

/// <summary>
/// A RefPool is specifically designed to efficiently work with a large number of structs.
/// The items are internally stored in one big array which improves memory locality when iterating
/// this container.
///
/// Removing elements leave holes in the internal array, so use IsOccupied to check if it is save to retrieve an element using
/// the indexing operator. The <see cref="RefPoolIterator{T}">RefPoolIterator</see> will do this for you.
///
/// Notes:
/// - When adding items you can use the `in` modifier to avoid unncessary copying of large structs.
/// - When retrieving items directly, or via the RefPoolIterator this collection returns a reference to avoud unnecessary copying and to make it easier to change the element
/// </summary>
/// <typeparam name="T">The element type, must be a struct</typeparam>
/// <example>
/// var pool = new RefPool&lt;int&gt;(3000);
/// pool.Add(3);
/// foreach(ref var element in pool)
/// {
///   // Do something
/// }
/// </example>
public sealed class RefPool<T> : IRefEnumerable<T>
    where T : struct
{
    private readonly BitArray Occupancy;
    private T[] pool;

    private int lowestUnusedSlot = 0;
    private int highestUsedSlot = -1;
    private int count;

    /// <summary>
    /// Create a new RefPool with the given initial capacity
    /// </summary>    
    public RefPool(int initialCapacity = 4)
    {
        this.Occupancy = new BitArray(initialCapacity);
        this.pool = new T[initialCapacity];
    }

    /// <summary>
    /// The current capacity
    /// </summary>
    public int Capacity => this.pool.Length;

    /// <summary>
    /// Retrieves the item at the given index and returns a reference to it
    /// </summary>        
    /// <exception cref="KeyNotFoundException">If there was no element at the given index</exception>
    public ref T this[int index]
    {
        get
        {
            if (this.Occupancy[index] == true)
            {
                return ref this.pool[index];
            }

            throw new KeyNotFoundException($"index {index} is not occupied in this pool");
        }
    }

    /// <summary>
    /// Adds the given element, add the in keyword at the caller to avoid unnecessary copying
    /// </summary>
    public int Add(in T item)
    {
        this.EnsureCapacity(this.count + 1);

        var index = this.lowestUnusedSlot;
        this.lowestUnusedSlot = this.IndexOfFirstUnused(this.lowestUnusedSlot + 1);
        this.highestUsedSlot = Math.Max(this.highestUsedSlot, index);

        this.Occupancy[index] = true;
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
            this.Occupancy[index] = false;

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
        return this.Occupancy[index];
    }

    /// <summary>
    /// Returns the enumerator, allows the user to use foreach(ref var x in this) on this collection
    /// </summary>    
    public IRefEnumerator<T> GetEnumerator()
    {
        return new RefPoolIterator<T>(this);
    }

    /// <summary>
    /// Moves items around to fill all holes in the underlying array
    /// </summary>
    /// <remarks>After calling this method the index return by the Add method is no longer valid</remarks>
    public void Defrag()
    {
        while (this.lowestUnusedSlot < this.highestUsedSlot)
        {
            this.pool[this.lowestUnusedSlot] = this.pool[this.highestUsedSlot];

            this.lowestUnusedSlot = this.IndexOfFirstUnused(this.lowestUnusedSlot + 1);
            this.highestUsedSlot = this.IndexOfLastUsed(this.highestUsedSlot - 1);
        }

        var list = new List<T>();
    }

    /// <summary>
    /// Removes all items from the collection
    /// </summary>
    public void Clear()
    {
        this.Occupancy.SetAll(false);
        this.lowestUnusedSlot = 0;
        this.highestUsedSlot = -1;
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
            this.Occupancy.Length = newCapacity;
        }
    }

    private int IndexOfFirstUnused(int minIndex)
    {
        if (minIndex < this.pool.Length)
        {
            for (var i = minIndex; i < this.pool.Length; i++)
            {
                if (this.Occupancy[i] == false)
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
                if (this.Occupancy[i] == true)
                {
                    return i;
                }
            }
        }

        return -1;
    }
}
