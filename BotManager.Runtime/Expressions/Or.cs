using System.Text.Json.Serialization;
using BotManager.Runtime.Converters;

namespace BotManager.Runtime.Expressions;

/// <summary>
/// <para>
/// Logical or operator. It returns <c>true</c> if any expression in this list is <c>true</c> or if the list is empty.
/// Execution is stopped, once the first entry returns <c>true</c>.
/// </para>
/// <para>
/// Return type is <see cref="bool"/>.
/// </para>
/// <example>
/// This json example returns <c>true</c>:
/// <code>
/// { "$Or": [ true, false ] }
/// </code>
/// This json example returns <c>false</c>:
/// <code>
/// { "$Or": [ false, false ] }
/// </code>
/// </example>
/// </summary>
[JsonConverter(typeof(ListConverter<Or>))]
[Serializable]
public sealed class Or : List<IExpression?>, IExpression
{
    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
    {
        if (Count == 0)
            return true;
        
        foreach (var operation in this)
        {
            var result = await context.ExecuteAsync<bool>(operation);
            if (result)
                return true;
        }

        return false;
    }
}