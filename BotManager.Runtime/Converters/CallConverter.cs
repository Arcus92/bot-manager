using System.Text.Json.Serialization;
using BotManager.Runtime.Expressions;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The <see cref="JsonConverter"/> for <see cref="Call"/>.
/// </summary>
public class CallConverter : StringConverterBase<Call>
{
    /// <inheritdoc />
    protected override Call From(string value) => new(value);

    /// <inheritdoc />
    protected override string To(Call value) => value.Name;
}