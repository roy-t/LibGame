namespace LibGame.Collections;

/// <summary>
/// An enumerator that allows users to access elements in the container by reference
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IStructEnumerator<T>
    where T : struct
{
    /// <inheritdoc/>
    ref T Current { get; }

    /// <inheritdoc/>
    bool MoveNext();

    /// <inheritdoc/>
    void Reset();
}

/// <summary>
/// An enumerable that exposes an enumerator allows users to access elements in the container by reference
/// </summary>
public interface IRefEnumerable<T>
    where T : struct
{
    /// <inheritdoc/>
    IStructEnumerator<T> GetEnumerator();
}
