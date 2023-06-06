using LibGame.Collections;

namespace LibGame.Tests.Collections;

public static class RefPoolTests
{
    [Fact(DisplayName = "Smoke test for `RefPool`")]
    public static void SmokeTest()
    {
        // Test initial capacity
        var pool = new RefPool<Vector3>(7);
        Equal(7, pool.Capacity);

        // Test occupancy
        for (var i = 0; i < pool.Capacity; i++)
        {
            False(pool.IsOccupied(i));
        }

        // Test adding, occupancy
        for (var i = 0; i < 10; i++)
        {
            var vector = new Vector3(i, i, i);
            var index = pool.Add(in vector);

            True(pool.IsOccupied(index));
            Equal(vector, pool[index]);
        }

        // Test iterator
        var count = 0;
        foreach (ref var vector in pool)
        {
            var expected = new Vector3(count, count, count);
            Equal(expected, vector);

            count++;
        }
        Equal(10, count);

        // Test removing/clearing and trimming/defragmenting
        pool.TrimExcess();
        var previousCapacity = pool.Capacity;

        True(pool.IsOccupied(3));
        pool.Remove(3);
        False(pool.IsOccupied(3));

        pool.TrimExcess();
        Equal(previousCapacity, pool.Capacity);

        pool.Defrag();
        pool.TrimExcess();
        InRange(pool.Capacity, 9, previousCapacity - 1);

        pool.Clear();
        NotEqual(0, pool.Capacity);

        pool.TrimExcess(1);
        Equal(1, pool.Capacity);

        pool.TrimExcess(0);
        Equal(0, pool.Capacity);
    }
}
