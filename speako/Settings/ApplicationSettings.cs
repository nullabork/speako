using Newtonsoft.Json;
using speako.Services.Providers;
using System.Security.RightsManagement;
using System.Collections.Generic;
using speako.Services.VoiceProfiles;
using System.Collections.ObjectModel;
using speako.Services.Auth;
using speako.Services.PostProcessors;
using speako.Services.PreProcessors;
using Amazon.Runtime.Internal.Transform;

namespace speako.Settings
{
  public class ApplicationSettings : JsonSerializable<ApplicationSettings>
  {
    public ObservableCollection<IAuthSettings> ProviderSettings { get; set; } = new ObservableCollection<IAuthSettings>();

    public ObservableCollection<VoiceProfile> ConfiguredVoices { get; set; } = new ObservableCollection<VoiceProfile>();

    public ObservableCollection<IPostProcessorSettings> PostProcessors { get; set; } = new ObservableCollection<IPostProcessorSettings>();

    public ObservableCollection<IPreProcessorSettings> PreProcessors { get; set; } = new ObservableCollection<IPreProcessorSettings>();

    protected override void AfterLoad()
    {
      ProcessProviders(); // Call ProcessProviders after the object is loaded
    }

    [JsonIgnore()]
    public Dictionary<string, IPreProcessorSettings> PreProcessorLookup { get; set; } = new Dictionary<string, IPreProcessorSettings>();

    [JsonIgnore()]
    public Dictionary<string, IPostProcessorSettings> PostProcessorLookup { get; set; } = new Dictionary<string, IPostProcessorSettings>();

    [JsonIgnore()]
    public Dictionary<string, IAuthSettings> ProviderSettingsLookup { get; set; } = new Dictionary<string, IAuthSettings>();

    public void ProcessProviders()
    {
      
      ProviderSettingsLookup.Clear();
      foreach (var provider in ProviderSettings)
      {
        provider.Init();
        ProviderSettingsLookup.Add(provider.GUID, provider);
      }

      PostProcessorLookup.Clear();
      foreach (var processor in PostProcessors)
      {
        processor.Init();
        PostProcessorLookup.Add(processor.GUID, processor);
      }

      PreProcessorLookup.Clear();
      foreach (var processor in PreProcessors)
      {
        processor.Init();
        PreProcessorLookup.Add(processor.GUID, processor);
      }
    }
  }
}
