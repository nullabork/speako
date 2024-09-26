using Newtonsoft.Json;

namespace speako.Common
{
  class Compare
  {
    public static bool ObjectsPropertiesEqual<T>(T obj1, T obj2)
    {
      var settings = new JsonSerializerSettings
      {
        Formatting = Formatting.None,
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
        {
          NamingStrategy = new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy()
        }
      };

      string json1 = JsonConvert.SerializeObject(obj1, settings);
      string json2 = JsonConvert.SerializeObject(obj2, settings);

      return json1 == json2;
    }

  }
}
