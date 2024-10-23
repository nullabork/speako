using Accord.IO;
using FastDeepCloner;
using Newtonsoft.Json;
using speako.Services.PreProcessors;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace speako.Services.PostProcessors.DiscordWebHook
{
  class DiscordWebHookProcessorSettings : IPostProcessorSettings, INotifyPropertyChanged
  {
    private IPostProcessor _processor;
   
    /** things to save that we care about **/
    public string NameFormat { get; set; }

    [JsonIgnore]
    public IEnumerable<string> NameFormats {  get; set; } = [
      TokenType.Name.GetToken(), 
      TokenType.ProfileName.GetToken(), 
      $"{TokenType.Name.GetToken()} {TokenType.ProfileName.GetToken()}", 
      $"{TokenType.ProfileName.GetToken()} {TokenType.Name.GetToken()}"];

    public string IconUrl { get; set; }

    public ObservableCollection<DiscordChannelWebHook> ChannelURLS { get; set; } = new ObservableCollection<DiscordChannelWebHook>();

    /** Generic boot strapping stuff */
    public string DisplayName => "DiscordWebHook Settings";
    public string GUID { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string CastType => this.GetType().ToString();
    public bool ProcessAfter { get; set; }


    /** events **/
    public event PropertyChangedEventHandler PropertyChanged;

    /** functions **/
    public IPostProcessorControl GetSettingsControl()
    {
      return new DiscordWebHookSettingsControl();
    }
    public IPostProcessorSettings Duplicate()
    {
      var cloned = DeepCloner.Clone(this);
      cloned.GUID = Guid.NewGuid().ToString();
      return cloned;
    }

    public async void Init()
    {
      _processor = null;
      _processor = GetPostProcessor();
      await _processor.Configure(this);
    }

    public IPostProcessor GetPostProcessor()
    {
      if (_processor == null)
      {
        _processor = new DiscordWebHookProcessor();
      }

      return _processor;
    }
  }
}
