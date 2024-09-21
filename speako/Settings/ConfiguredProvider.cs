
using Newtonsoft.Json;
using speako.Services.Auth;
using speako.Services.Providers;

namespace speako.Settings
{
  public class ConfiguredProvider: IConfig
  {

    public string? ProviderName { get; set;}

    [JsonIgnore]
    public ITTSProvider Provider { set; get; }

    public IAuthSettings authSettings { get; set; }

    public string GUID { get; set; } = Guid.NewGuid().ToString();

    public override string ToString()
    {
      return Provider.Name;
    } 
  }
}
