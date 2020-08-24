using C1.DataCollection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FlexGridExplorer.Pages
{
    public class SortDescriptionJsonConverter : JsonConverter<SortDescription>
    {
        public override SortDescription Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            var direction = SortDirection.Ascending;
            string sortPath = null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return new SortDescription(sortPath, direction);
                }
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case nameof(SortDescription.SortPath):
                            sortPath = reader.GetString();
                            break;
                        case nameof(SortDescription.Direction):
                            direction = (SortDirection)reader.GetInt32();
                            break;
                    }
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, SortDescription value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value);
        }
    }
}
