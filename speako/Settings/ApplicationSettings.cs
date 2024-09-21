using Newtonsoft.Json;
using speako.Services.Providers;
using System.Security.RightsManagement;
using System.Collections.Generic;
using speako.Services.VoiceSettings;
using System.Collections.ObjectModel;
using speako.Services.Auth;

namespace speako.Settings
{
  public class ApplicationSettings : JsonSerializable<ApplicationSettings>
  {
    public ObservableCollection<IAuthSettings> ConfiguredProviders { get; set; } = new ObservableCollection<IAuthSettings>();
    public ObservableCollection<VoiceProfile> ConfiguredVoices { get; set; } = new ObservableCollection<VoiceProfile>();

    protected override void AfterLoad(ApplicationSettings instance)
    {
      ProcessProviders(); // Call ProcessProviders after the object is loaded
    }

    public void ProcessProviders()
    {
      foreach (var provider in ConfiguredProviders)
      {
        provider.InitProvider();
      }
    }

    //private void processProvider(ConfiguredProvider provider)
    //{
    //  var providerType = Type.GetType(provider.ProviderName);
    //  if (providerType == null)
    //  {
    //    return;
    //  }

    //  var providerInstance = (ITTSProvider)Activator.CreateInstance(providerType);
    //  providerInstance.LoadSettings(provider);
    //  provider.Provider = providerInstance;
    //}

    //public void AddConfiguredProvider(ConfiguredProvider provider)
    //{
    //  ConfiguredProviders.Add(provider);
    //  processProvider(provider);
    //}

  }
}
