
using Newtonsoft.Json;
using speako.Services.Providers;

namespace speako.Settings
{
  public class ConfiguredProvider: IConfig
  {

    public string? Name { get; set; }

    public string? ProviderName { get; set;}

    [JsonIgnore]
    public ITTSProvider Provider { set; get; }

    public string uuid { get; set; } = Guid.NewGuid().ToString();

    public override string ToString()
    {
      return Name;
    }
  }
}
