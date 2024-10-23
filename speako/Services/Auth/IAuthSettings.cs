using speako.Services.Providers;
using System.ComponentModel;

namespace speako.Services.Auth
{
  public interface IAuthSettings
  {
    public string DisplayName { get; }
    string GUID { get; set; }
    string Name { get; set; }
    string CastType { get; set; }
 
    public ITTSProvider GetProvider();

    void Init();

    public IProviderSettingsControl SettingsControl();

    public bool IsConfigured();


    public event PropertyChangedEventHandler PropertyChanged;
  }
}
