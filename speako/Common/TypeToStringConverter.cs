using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Common
{
  public class TypeToStringConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      // We allow this converter for any object
      return true;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value != null)
      {
        // Serialize the type of the object as a string
        writer.WriteValue(value.GetType().ToString());
      }
      else
      {
        writer.WriteNull();
      }
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      // Deserialize the type as a string
      var typeName = reader.Value.ToString();
      var type = Type.GetType(typeName);

      // If we can get the type, instantiate it
      if (type != null)
      {
        // Populate the object with default constructor
        return Activator.CreateInstance(type);
      }

      throw new JsonSerializationException($"Could not resolve type: {typeName}");
    }
  }
}
