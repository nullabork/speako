using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Common
{
  public class DateTimeCaster : JsonConverter<DateTime>
  {
    public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.String)
      {
        var dateTimeString = reader.Value?.ToString();
        if (DateTime.TryParse(dateTimeString, out var parsedDateTime))
        {
          return parsedDateTime;
        }
      }
      return existingValue; // Or return DateTime.MinValue if necessary
    }

    public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
    {
      writer.WriteValue(value.ToString("o")); // Use the round-trip date/time format for JSON
    }
  }

}
