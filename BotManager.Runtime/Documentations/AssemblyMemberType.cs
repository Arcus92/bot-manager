namespace BotManager.Runtime.Documentations;

/// <summary>
/// The type of a <see cref="AssemblyMember"/>.
/// </summary>
public enum AssemblyMemberType
{
    /// <summary>
    /// The member type for classes.
    /// </summary>
    Type,
    
    /// <summary>
    /// The member type for methods.
    /// </summary>
    Method,
    
    /// <summary>
    /// The member type for fields.
    /// </summary>
    Field,
    
    /// <summary>
    /// The member type for properties.
    /// </summary>
    Property,
}