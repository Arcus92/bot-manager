using System.Text.Json.Serialization;
using BotManager.Runtime.Expressions;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The <see cref="JsonConverter"/> for <see cref="Error"/>.
/// </summary>
public sealed class ErrorConverter : ExpressionConverterBase<Error>
{
    /// <inheritdoc />
    protected override Error From(IExpression? value) => new(value);

    /// <inheritdoc />
    protected override IExpression? To(Error value) => value.Message;
}