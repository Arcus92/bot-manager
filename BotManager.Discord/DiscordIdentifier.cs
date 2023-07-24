using Discord;

namespace BotManager.Discord;

/// <summary>
/// Identifies a guild, channel or user either by name or id.
/// </summary>
public readonly struct DiscordIdentifier
{
    /// <summary>
    /// Creates the identifier by an unknown type.
    /// </summary>
    /// <param name="value">The name or id.</param>
    /// <exception cref="ArgumentException"></exception>
    public DiscordIdentifier(object? value)
    {
        if (value is null)
        {
            Name = null;
            Id = 0;
        }
        else if (value is string s)
        {
            Name = s;
            Id = 0;
        }
        else if (value is ulong l)
        {
            Name = null;
            Id = l;
        }
        else throw new ArgumentException($"Unsupported type '{value.GetType()}'.");
    }
    
    /// <summary>
    /// Creates the identifier by a name.
    /// </summary>
    /// <param name="name">The name.</param>
    public DiscordIdentifier(string name)
    {
        Name = name;
        Id = 0;
    }
    
    /// <summary>
    /// Creates the identifier by an id number.
    /// </summary>
    /// <param name="id">The id number.</param>
    public DiscordIdentifier(ulong id)
    {
        Name = null;
        Id = id;
    }
    
    /// <summary>
    /// Gets the name. If this is <c>null</c>, the <see cref="Id"/> is used instead.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Gets the id.
    /// </summary>
    public ulong Id { get; }

    /// <summary>
    /// Returns the name of the identifier.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (Name is not null)
            return Name;
        return Id.ToString();
    }

    /// <summary>
    /// Gets the guild
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T? GetGuild<T>(IEnumerable<T> list) where T : IGuild
    {
        var name = Name;
        var id = Id;
        if (name is not null)
            return list.FirstOrDefault(g => g.Name == name);
        return list.FirstOrDefault(g => g.Id == id);
    }
    
    /// <summary>
    /// Gets the guild
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T? GetChannel<T>(IEnumerable<T> list) where T : IChannel
    {
        var name = Name;
        var id = Id;
        if (name is not null)
            return list.FirstOrDefault(g => g.Name == name);
        return list.FirstOrDefault(g => g.Id == id);
    }
    
    /// <summary>
    /// Gets the guild
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T? GetUser<T>(IEnumerable<T> list) where T : IUser
    {
        var name = Name;
        var id = Id;
        if (name is not null)
            return list.FirstOrDefault(g => g.Username == name);
        return list.FirstOrDefault(g => g.Id == id);
    }
}