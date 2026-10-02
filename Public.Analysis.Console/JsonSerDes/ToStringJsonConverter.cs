using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Public.Analysis.Console.JsonSerDes
{
    public class ToStringJsonConverter<T> : JsonConverter<T>
    {
        // Called when serializing the class to JSON
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                // This calls the custom ToString() method of your class
                writer.WriteStringValue(value.ToString());
            }
        }

        // Called when deserializing JSON back to the class (Optional)
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // If you only need one-way serialization, throw NotImplementedException
            throw new NotImplementedException("Deserialization is not supported for this type.");
        }
    }
}
