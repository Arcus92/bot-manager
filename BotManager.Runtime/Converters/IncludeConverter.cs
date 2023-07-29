using System.Text.Json.Serialization;
using BotManager.Runtime.Expressions;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The <see cref="JsonConverter"/> for <see cref="Include"/>.
/// </summary>
public class IncludeConverter : StringConverterBase<Include>
{
    /// <inheritdoc />
    protected override Include From(string value) => new(value);

    /// <inheritdoc />
    protected override string To(Include value) => value.FileName;
}