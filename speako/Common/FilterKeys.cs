using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace speako.Common
{

  public enum FilterAction
  {
    INCLUDE,
    EXCLUDE
  }
  public class FilterKeys : JsonConverter
  {
    private readonly HashSet<string> _keysToFilter;
    private readonly FilterAction _action;

    public FilterKeys(IEnumerable<string> keys, FilterAction action)
    {
      _keysToFilter = new HashSet<string>(keys);
      _action = action;
    }

    public static JsonSerializerSettings Include(IEnumerable<String> keys)
    {
      var settings = new JsonSerializerSettings
      {
        Converters = new List<JsonConverter> { new FilterKeys(keys, FilterAction.INCLUDE) },
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
      };

      return settings;
    }

    public static JsonSerializerSettings Exclude(IEnumerable<String> keys)
    {
      var settings = new JsonSerializerSettings
      {
        Converters = new List<JsonConverter> { new FilterKeys(keys, FilterAction.EXCLUDE) },
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
      };

      return settings;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      var token = JToken.FromObject(value);
      if (token.Type == JTokenType.Object)
      {
        var filteredObject = new JObject();
        foreach (var property in (JObject)token)
        {
          if (FilterAction.INCLUDE == _action && _keysToFilter.Contains(property.Key))
          {
            filteredObject.Add(property.Key, property.Value);
          }

          if (FilterAction.EXCLUDE == _action && !_keysToFilter.Contains(property.Key))
          {
            filteredObject.Add(property.Key, property.Value);
          }
        }
        filteredObject.WriteTo(writer);
      }
      else
      {
        token.WriteTo(writer); // In case it's not an object
      }
    }

    public override bool CanRead => false;

    public override bool CanConvert(Type objectType)
    {
      return true; // Apply to all types
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      throw new NotImplementedException("Only serialization is supported by IncludeKeys.");
    }
  }




}
