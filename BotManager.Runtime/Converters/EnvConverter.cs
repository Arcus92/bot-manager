using System.Text.Json.Serialization;
using BotManager.Runtime.Expressions;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The <see cref="JsonConverter"/> for <see cref="Env"/>.
/// </summary>
public class EnvConverter : StringConverterBase<Env>
{
    /// <inheritdoc />
    protected override Env From(string value) => new(value);

    /// <inheritdoc />
    protected override string To(Env value) => value.Name;
}