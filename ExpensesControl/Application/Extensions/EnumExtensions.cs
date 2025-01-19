using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExpensesControl.Application.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Custom JSON converter to serialize enums using their Description attribute.
    /// </summary>
    public class DescriptionEnumConverter<T> : JsonConverter<T> where T : Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException("Deserialization of enums with descriptions is not supported.");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var description = value.GetDescription();
            writer.WriteStringValue(description);
        }
    }
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();

        return attribute?.Description ?? value.ToString();
    }
}
