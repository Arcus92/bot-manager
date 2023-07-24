using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// Executes and returns a random item from this list.
/// <para>
/// Returns: Type of the chosen item.
/// </para>
/// </summary>
[JsonConverter(typeof(ListConverter<Choose>))]
public sealed class Choose : List<IExpression?>, IExpression
{
    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        if (Count == 0)
            return null;

        var i = Random.Shared.Next(Count);
        return await context.ExecuteAsync(this[i], returnType);
    }
}