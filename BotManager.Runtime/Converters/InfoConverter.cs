using System.Text.Json.Serialization;
using BotManager.Runtime.Expressions;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The <see cref="JsonConverter"/> for <see cref="Info"/>.
/// </summary>
public class InfoConverter : ExpressionConverterBase<Info>
{
    /// <inheritdoc />
    protected override Info From(IExpression? value) => new(value);

    /// <inheritdoc />
    protected override IExpression? To(Info value) => value.Message;
}