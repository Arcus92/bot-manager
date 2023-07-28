using System.Diagnostics.CodeAnalysis;

namespace BotManager.Runtime;

/// <summary>
/// A dictionary with text keys to store all kind of data. Used for the variables in <see cref="RuntimeContext"/>.
/// <typeparam name="T">The value type for the storage objects.</typeparam>
/// </summary>
public class Storage<T>
{
    /// <summary>
    /// Creates an empty storage.
    /// </summary>
    public Storage()
    {
        _dictionary = new();
    }

    /// <summary>
    /// Copies the <paramref name="original"/> storage into this one.
    /// </summary>
    /// <param name="original">The original storage to copy from.</param>
    public Storage(Storage<T> original)
    {
        _dictionary = new(original._dictionary);
    }

    /// <summary>
    /// The internal dictionary.
    /// </summary>
    private readonly Dictionary<string, T> _dictionary;

    /// <summary>
    /// Sets the value with the given key. Overwrites existing entries.
    /// If <paramref name="value"/> is <c>null</c>, it will remove the entry.
    /// </summary>
    /// <param name="key">The key of the entry.</param>
    /// <param name="value">The value to set.</param>
    public void Set(string key, T? value)
    {
        if (value is null)
        {
            _dictionary.Remove(key);
            return;
        }

        _dictionary[key] = value;
    }

    /// <summary>
    /// Gets a value with the given <paramref name="key"/>.
    /// Throws an exception if the key doesn't exist. 
    /// </summary>
    /// <param name="key">The key of the entry.</param>
    /// <returns></returns>
    public T Get(string key)
    {
        return _dictionary[key];
    }
    
    /// <summary>
    /// Gets a value with the given <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key of the entry.</param>
    /// <param name="value">Returns the found value.</param>
    /// <returns>Returns if the value was found.</returns>
    public bool TryGet(string key, [MaybeNullWhen(false)] out T value)
    {
        return _dictionary.TryGetValue(key, out value);
    }
    
    /// <summary>
    /// Gets a value with the given <paramref name="key"/> and of type <typeparamref name="TValue"/>.
    /// </summary>
    /// <param name="key">The key of the entry.</param>
    /// <param name="value">Returns the found value.</param>
    /// <returns>Returns if the value was found and it matches the type.</returns>
    public bool TryGet<TValue>(string key, [MaybeNullWhen(false)] out TValue value)
    {
        value = default;
        if (!_dictionary.TryGetValue(key, out var originalValue))
            return false;
        
        if (originalValue is not TValue typeValue) 
            return false;
        
        value = typeValue;
        return true;
    }
}