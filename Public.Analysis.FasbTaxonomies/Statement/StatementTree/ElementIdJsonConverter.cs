using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Public.Analysis.FasbTaxonomies.Statement.StatementTree
{
    public class ElementIdJsonConverter : JsonConverter<ElementId>
    {
        public override ElementId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? keyString = reader.GetString();
            if (keyString is null)
            {
                return null;
            }
            return new ElementId(keyString);
        }

        public override void Write(Utf8JsonWriter writer, ElementId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.elementId);
        }

        public override ElementId ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return this.Read(ref reader, typeToConvert, options) ?? ElementId.Empty;
        }

        public override void WriteAsPropertyName(Utf8JsonWriter writer, [DisallowNull] ElementId value, JsonSerializerOptions options)
        {
            writer.WritePropertyName(value.elementId);
        }
    }
}
