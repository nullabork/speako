using Accord.Math;
using Newtonsoft.Json;
using Omu.ValueInjecter;
using speako.Common;
using speako.Services.Auth;
using speako.Services.PostProcessors.Discord;
using System.ComponentModel;

namespace speako.Services.Providers.Piper
{

  public class PiperTTSAuthSettings : IAuthSettings, INotifyPropertyChanged
  {

    public string DisplayName => "Piper TTS";


    public event PropertyChangedEventHandler PropertyChanged;

    public IProviderSettingsControl SettingsControl()
    {
      return new PiperTTSSettingsControl();
    }

    public PiperTTSAuthSettings()
    {
      GUID = Guid.NewGuid().ToString();
      CastType = this.GetType().ToString();
    }

    [JsonIgnore()]
    public ITTSProvider Provider { get; set; }


    
    public string Name { get; set; } = "PiperTTS";
    public string CastType { get; set; }

    public string GUID { get; set; }

    public void Init()
    {
      _provider = new PiperTTSProvider();
      _provider.Configure(this);
    }

    //this is like... do the fields at least look filled out
    public bool IsConfigured()
    {

      return true;
    }

    ITTSProvider _provider { get; set; }

    public ITTSProvider GetProvider()
    {
      if (_provider == null)
      {
        _provider = new PiperTTSProvider();
      }

      return _provider;
    }
  }
}
