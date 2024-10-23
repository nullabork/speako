using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
namespace speako.Common
{
  public class JsonCaster : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {

      if (objectType.Name == "String")
      {
        return false;
      }


      if (objectType.IsValueType)
      {
        return false;
      }

      // Check if the objectType is a generic type and if it is an ObservableCollection<>
      if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(ObservableCollection<>))
      {
        return false; // Do not convert ObservableCollection<>
      }

      return true; // Convert all other types
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {

      if (reader.TokenType == JsonToken.StartObject)
      {
        JObject jObject = JObject.Load(reader);

        // Check if CastType is provided and is valid
        if (jObject["CastType"] != null && !string.IsNullOrWhiteSpace(jObject["CastType"].ToString()))
        {
          string typeName = jObject["CastType"].ToString();
          Type castType = Type.GetType(typeName);

          if (castType != null)
          {
            // Create an instance of the specified type
            object instance = Activator.CreateInstance(castType);

            // Populate the instance with the JSON data
            //reader

            var b = jObject.ToString();
            JsonConvert.PopulateObject(b, instance);

            return instance;
          }
        }
        //Nope
        //return existingValue ?? Activator.CreateInstance(objectType);
      }

      return existingValue;
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new NotImplementedException("Unnecessary because CanWrite is false.");
    }
  }
}
