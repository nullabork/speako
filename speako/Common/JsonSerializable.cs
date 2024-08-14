﻿using Newtonsoft.Json;

public abstract class JsonSerializable<T> where T : JsonSerializable<T>, new()
{
  public void Save()
  {
    var json = JsonConvert.SerializeObject(this, Formatting.Indented);
    json = BeforeSave((T)this, json);
    File.WriteAllText(FilePath, json);
    AfterSave((T)this);
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
      JsonConvert.PopulateObject(json, this);
      AfterLoad((T)this);
    }
  }

  protected virtual void AfterLoad(T instance)
  {
    // Default implementation does nothing
  }

  protected virtual void AfterSave(T instance)
  {
    // Default implementation does nothing
  }

  protected virtual string BeforeSave(T instance, string serialized)
  {
    // Default implementation does nothing
    return serialized;
  }
}
