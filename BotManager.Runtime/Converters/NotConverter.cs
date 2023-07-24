using System.Text.Json.Serialization;
using BotManager.Runtime.Expressions;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The <see cref="JsonConverter"/> for <see cref="Not"/>.
/// </summary>
public class NotConverter : ExpressionConverterBase<Not>
{
    /// <inheritdoc />
    protected override Not From(IExpression? value) => new(value);

    /// <inheritdoc />
    protected override IExpression? To(Not value) => value.Expression;
}