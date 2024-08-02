using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class IsoDateTimeConverter : JsonConverter<DateTime>
{
  public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    if (reader.TokenType == JsonTokenType.String)
    {
      if (DateTime.TryParse(reader.GetString(), out DateTime dateTime))
      {
        return dateTime;
      }
    }

    return DateTime.MinValue; // Handle conversion error gracefully
  }

  public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")); // Convert DateTime to ISO format
  }
}
