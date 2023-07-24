using System.Text.Json;
using System.Text.Json.Serialization;
using BotManager.Runtime.Expressions;
using Boolean = BotManager.Runtime.Expressions.Boolean;
using Int32 = BotManager.Runtime.Expressions.Int32;
using String = BotManager.Runtime.Expressions.String;

namespace BotManager.Runtime.Converters;

/// <summary>
/// The <see cref="JsonConverter"/> for <see cref="IExpression"/>.
/// </summary>
public class ExpressionConverter : JsonConverter<IExpression?>
{
    /// <inheritdoc />
    public override IExpression? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        IExpression? result = null;
        
        #region Null
        
        if (reader.TokenType == JsonTokenType.Null)
            return null;
        
        #endregion Null
        
        #region Static types

        if (reader.TokenType == JsonTokenType.String)
        {
            var s = reader.GetString();
            if (s is null) throw new ArgumentException("Unexpected null string!"); // Should not happen.
            return new String(s);
        }

        if (reader.TokenType == JsonTokenType.Number)
        {
            return new Int32(reader.GetInt32());
        }
        
        if (reader.TokenType == JsonTokenType.True)
        {
            return new Boolean(true);
        }
        
        if (reader.TokenType == JsonTokenType.False)
        {
            return new Boolean(false);
        }
        
        #endregion Static types

        #region Lists
        
        // Handle lists
        if (reader.TokenType == JsonTokenType.StartArray)
            return ListConverter<List>.StaticRead(ref reader, options);
        
        #endregion Lists
        
        #region Objects
        
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Unknown token. Expected object start.");
        
        while (reader.Read())
        {
            // End
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            // Property name
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                // There should be a single property starting with an @ followed by the type name to create.
                var propertyName = reader.GetString();
                if (propertyName is null || !propertyName.StartsWith("@"))
                    throw new JsonException($"Unknown property '{propertyName}'. Expected '@{{typeName}}'.");

                if (result is not null)
                    throw new JsonException("Multiple '@{typeName}' properties are not supported.");
                
                var typeName = propertyName[1..];

                if (!IExpression.TryGetExpressionType(typeName, out var type))
                    throw new JsonException($"Unknown type '{typeName}'.");

                // Deserialize the inner objects
                result = (IExpression?)JsonSerializer.Deserialize(ref reader, type, options);
            }
            else
            {
                throw new JsonException("Unknown token. Expected property with '@{typeName}'.");
            }
        }

        return result;
        
        #endregion Options
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, IExpression? value, JsonSerializerOptions options)
    {
        #region Null
        
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }
        
        #endregion Null
        
        #region Static types
        
        if (value is String t)
        {
            writer.WriteStringValue(t.Value);
            return;
        }
        
        if (value is Int32 i)
        {
            writer.WriteNumberValue(i.Value);
            return;
        }

        if (value is Boolean b)
        {
            writer.WriteBooleanValue(b.Value);
            return;
        }
        
        #endregion Static types
        
        #region Lists
        
        if (value is List list)
        {
            ListConverter<List>.StaticWrite(writer, list, options);
            return;
        }
        
        #endregion Lists
        
        #region Objects
        
        var type = value.GetType();
        
        writer.WriteStartObject();
        
        writer.WritePropertyName($"@{type.Name}");
        JsonSerializer.Serialize(writer, value, type, options);
        
        writer.WriteEndObject();
        
        #endregion Objects
    }
}