using System.Text.Json.Serialization;

namespace BotManager.Runtime;

/// <summary>
/// Defines a public property for an <see cref="IExpression"/>.
/// </summary>
public class InputAttribute : Attribute
{
    /// <summary>
    /// Gets and sets if this property is the main content property. Set this if the parent expression has a
    /// <see cref="JsonConverter"/> that writes this property value directly into the objects content.
    /// This is mostly used for single property expressions.
    /// </summary>
    public bool ContentProperty { get; set; }
}