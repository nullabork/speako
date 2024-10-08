﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using speako.Common;

public abstract class JsonSerializable<T> where T : JsonSerializable<T>, new()
{
  public void Save()
  {
    var json = JsonConvert.SerializeObject(this, Formatting.Indented);
    File.WriteAllText(FilePath, json);
  }

  public static string GetFilePath(string filename)
  {
    return Path.GetFullPath(Path.Combine("Config", filename + ".json"));
  }

  [JsonIgnore]
  public string FilePath { get; private set; }

  public static T Load()
  {
    return Load(typeof(T).Name);
  }

  public static T Load(string filename)
  {
    var instance = new T
    {
      FilePath = GetFilePath(filename)
    };
    instance.LoadFile();
    return instance;
  }

  public void LoadFile()
  {
    if (File.Exists(FilePath))
    {
      var json = File.ReadAllText(FilePath);

      var settings = new JsonSerializerSettings
      {
        Converters = new List<JsonConverter> { new JsonCaster() }
      };

      JsonConvert.PopulateObject(json, this, settings);
      AfterLoad();
    }
  }
  
  protected virtual void AfterLoad()
  {
    // Default implementation does nothing
  }
}
