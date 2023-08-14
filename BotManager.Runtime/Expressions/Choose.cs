using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// <para>
/// Executes and returns a random item from this list.
/// </para>
/// <para>
/// Return type is the type of the chosen item.
/// </para>
/// <example>
/// This json example returns either <c>1</c>, <c>2</c> or <c>3</c> chosen randomly:
/// <code>
/// { "Choose": [ 1, 2, 3 ] }
/// </code>
/// </example>
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