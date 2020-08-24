using C1.DataCollection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FlexGridExplorer.Pages
{
    public class FilterExpressionJsonConverter : JsonConverter<FilterExpression>
    {
        private const string Kind = "Kind";

        enum FilterExpressionKind
        {
            Binary = 1,
            Not = 2,
            Operation = 3,
            Text = 4,
        }

        public override bool CanConvert(Type typeToConvert) =>
            typeof(FilterExpression).IsAssignableFrom(typeToConvert);

        public override FilterExpression Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            string propertyName = reader.GetString();
            if (propertyName != Kind)
                throw new JsonException();

            reader.Read();
            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException();

            var expressionKind = Enum.Parse<FilterExpressionKind>(reader.GetString());
            FilterCombination filterCombination = default(FilterCombination);
            FilterExpression leftExpression = null, rightExpression = null, subExpression = null;
            string filterPath = null, value = null;
            FilterOperation filterOperation = default(FilterOperation);
            bool matchCase = false, matchWholeWord = false;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    switch (expressionKind)
                    {
                        case FilterExpressionKind.Binary:
                            return new FilterBinaryExpression(filterCombination, leftExpression, rightExpression);
                        case FilterExpressionKind.Not:
                            return new FilterNotExpression(subExpression);
                        case FilterExpressionKind.Text:
                            return new FilterTextExpression(filterPath, filterOperation, value, matchCase, matchWholeWord);
                        case FilterExpressionKind.Operation:
                            return new FilterOperationExpression(filterPath, filterOperation, value);
                    }
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    propertyName = reader.GetString();
                    reader.Read();
                    switch (expressionKind)
                    {
                        case FilterExpressionKind.Binary:
                            switch (propertyName)
                            {
                                case nameof(FilterBinaryExpression.FilterCombination):
                                    filterCombination = Enum.Parse<FilterCombination>(reader.GetString());
                                    break;
                                case nameof(FilterBinaryExpression.LeftExpression):
                                    leftExpression = JsonSerializer.Deserialize<FilterExpression>(ref reader, options);
                                    break;
                                case nameof(FilterBinaryExpression.RightExpression):
                                    rightExpression = JsonSerializer.Deserialize<FilterExpression>(ref reader, options);
                                    break;
                            }
                            break;
                        case FilterExpressionKind.Not:
                            subExpression = JsonSerializer.Deserialize<FilterExpression>(ref reader, options);
                            break;
                        case FilterExpressionKind.Text:
                            switch (propertyName)
                            {
                                case nameof(FilterTextExpression.FilterPath):
                                    filterPath = reader.GetString();
                                    break;
                                case nameof(FilterTextExpression.FilterOperation):
                                    filterOperation = Enum.Parse<FilterOperation>(reader.GetString());
                                    break;
                                case nameof(FilterTextExpression.Value):
                                    value = reader.GetString();
                                    break;
                                case nameof(FilterTextExpression.MatchCase):
                                    matchCase = reader.GetBoolean();
                                    break;
                                case nameof(FilterTextExpression.MatchWholeWord):
                                    matchWholeWord = reader.GetBoolean();
                                    break;
                            }
                            break;
                        case FilterExpressionKind.Operation:
                            switch (propertyName)
                            {
                                case nameof(FilterOperationExpression.FilterPath):
                                    filterPath = reader.GetString();
                                    break;
                                case nameof(FilterOperationExpression.FilterOperation):
                                    filterOperation = Enum.Parse<FilterOperation>(reader.GetString());
                                    break;
                                case nameof(FilterOperationExpression.Value):
                                    value = reader.GetString();
                                    break;
                            }
                            break;
                    }
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, FilterExpression expression, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (expression is FilterBinaryExpression binaryExpression)
            {
                writer.WriteString(Kind, nameof(FilterExpressionKind.Binary));
                writer.WriteString(nameof(FilterBinaryExpression.FilterCombination), binaryExpression.FilterCombination.ToString());
                writer.WritePropertyName(nameof(FilterBinaryExpression.LeftExpression));
                JsonSerializer.Serialize(writer, binaryExpression.LeftExpression, options);
                writer.WritePropertyName(nameof(FilterBinaryExpression.RightExpression));
                JsonSerializer.Serialize(writer, binaryExpression.RightExpression, options);
            }
            else if (expression is FilterNotExpression notExpression)
            {
                writer.WriteString(Kind, nameof(FilterExpressionKind.Not));
            }
            else if (expression is FilterTextExpression textExpression)
            {
                writer.WriteString(Kind, nameof(FilterExpressionKind.Text));
                writer.WriteString(nameof(FilterTextExpression.FilterPath), textExpression.FilterPath);
                writer.WriteString(nameof(FilterTextExpression.FilterOperation), textExpression.FilterOperation.ToString());
                writer.WriteString(nameof(FilterTextExpression.Value), textExpression.Value?.ToString() ?? "");
                writer.WriteBoolean(nameof(FilterTextExpression.MatchCase), textExpression.MatchCase);
                writer.WriteBoolean(nameof(FilterTextExpression.MatchWholeWord), textExpression.MatchWholeWord);
            }
            else if (expression is FilterOperationExpression operationExpression)
            {
                writer.WriteString(Kind, nameof(FilterExpressionKind.Operation));
                writer.WriteString(nameof(FilterOperationExpression.FilterPath), operationExpression.FilterPath);
                writer.WriteString(nameof(FilterOperationExpression.FilterOperation), operationExpression.FilterOperation.ToString());
                writer.WriteString(nameof(FilterOperationExpression.Value), operationExpression.Value?.ToString() ?? "");
            }

            writer.WriteEndObject();
        }
    }
}
