using System.Text.Json.Serialization;
using String = BotManager.Runtime.Expressions.String;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The <see cref="JsonConverter"/> for <see cref="Expressions.String"/>.
/// </summary>
public sealed class StringConverter : StringConverterBase<String>
{
    /// <inheritdoc />
    protected override String From(string value) => new(value);

    /// <inheritdoc />
    protected override string To(String value) => value.Value;
}