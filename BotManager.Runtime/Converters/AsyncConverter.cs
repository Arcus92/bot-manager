using System.Text.Json.Serialization;
using BotManager.Runtime.Expressions;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The <see cref="JsonConverter"/> for <see cref="Async"/>.
/// </summary>
public class AsyncConverter : ExpressionConverterBase<Async>
{
    /// <inheritdoc />
    protected override Async From(IExpression? value) => new(value);

    /// <inheritdoc />
    protected override IExpression? To(Async value) => value.Expression;
}