using System.Text.Json.Serialization;
using BotManager.Runtime.Expressions;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The <see cref="JsonConverter"/> for <see cref="Env"/>.
/// </summary>
public sealed class GetConverter : StringConverterBase<Get>
{
    /// <inheritdoc />
    protected override Get From(string value) => new(value);

    /// <inheritdoc />
    protected override string To(Get value) => value.Name;
}