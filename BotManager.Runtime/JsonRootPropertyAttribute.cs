using System.Text.Json.Serialization;

namespace BotManager.Runtime;

/// <summary>
/// Add this attribute to a property in your serializable class, if the property is it's only property .
/// If this is present in a class, the json object with the property list is skipped and only the value of this
/// property is returned.
/// You will still need a <see cref="JsonConverter"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class JsonRootPropertyAttribute : Attribute
{
}