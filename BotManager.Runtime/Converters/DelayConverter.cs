using System.Text.Json.Serialization;
using BotManager.Runtime.Expressions;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The <see cref="JsonConverter"/> for <see cref="Delay"/>.
/// </summary>
public sealed class DelayConverter : ExpressionConverterBase<Delay>
{
    /// <inheritdoc />
    protected override Delay From(IExpression? value) => new(value);

    /// <inheritdoc />
    protected override IExpression? To(Delay value) => value.Milliseconds;
}