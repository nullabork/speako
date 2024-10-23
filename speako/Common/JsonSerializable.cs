using Newtonsoft.Json;

using speako.Common;

public abstract class JsonSerializable<T> where T : JsonSerializable<T>, new()
{

  public event EventHandler<T> Saved;
  public event EventHandler<T> Loaded;

  public string FileName { get; set; }

  public void Save()
  {
    var json = JsonConvert.SerializeObject(this, Formatting.Indented);
    File.WriteAllText(GetFilePath(), json);
    if (Saved != null)
    {
      Saved.Invoke(this, (T)this);
    }
  }


  public string GetFileBase()
  {
    return Path.GetFullPath("Config");
  }

  public string GetFilePath()
  {
    if (string.IsNullOrEmpty(FileName))
    {
      FileName = GetFileName();
    }

    return Path.Combine(GetFileBase(), FileName + ".json");
  }
  
  public string GetFileName()
  {
    return typeof(T).Name;
  } 

  public async Task Load()
  {
    var filePath = GetFilePath();

    if (File.Exists(filePath))
    {
      var json = await File.ReadAllTextAsync(filePath);

      var settings = new JsonSerializerSettings
      {
        Converters = [new JsonCaster()],

      };

      var messageSession = JsonConvert.DeserializeObject<T>(json, settings);

      JsonConvert.PopulateObject(json, this, settings);
      AfterLoad();
      if (Loaded != null)
      {
        Loaded.Invoke(this, (T)this);
      }
    }
  }

  protected virtual void AfterLoad()
  {
    // Default implementation does nothing
  }
}
