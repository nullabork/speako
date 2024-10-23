using Accord.IO;
using FastDeepCloner;
using speako.Services.PreProcessors;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace speako.Services.PostProcessors.Discord
{
  class DiscordProcessorSettings : IPostProcessorSettings, INotifyPropertyChanged
  {
    public string DisplayName => "Discord Settings";

    public IPostProcessorControl GetSettingsControl()
    {
      return new DiscordSettingsControl();
    }

    private IPostProcessor _processor;
     
    public IPostProcessor GetPostProcessor()
    {
      if (_processor == null) {
        _processor = new DiscordProcessor();
      }

      return _processor;
    }

    public async void Init()
    {
      _processor = null;
      _processor = GetPostProcessor();
      _processor.Configure(this);
    }

    //just unique id used for references
    public string GUID { get; set; } = Guid.NewGuid().ToString();

    //Name purely used for display in the UI
    public string Name { get; set; }

    //Actual config for Discord.net
    public string BotToken { get; set; }

    //Actual config for Discord.net
    public ObservableCollection<DiscordChannel> ChannelIds { get; set; } = new ObservableCollection<DiscordChannel>();


    //Cast type is used by the newtonSoft json so it knows what class to create when deserialising
    public string CastType => this.GetType().ToString();

    //Is the process function going to run as the voice is still playing?
    public bool ProcessAfter { get; set; }

    //helps with the window controls
    public event PropertyChangedEventHandler PropertyChanged;

    public IPostProcessorSettings Duplicate()
    {
      var cloned = DeepCloner.Clone(this);
      cloned.GUID = Guid.NewGuid().ToString();
      return cloned;
    }
  }
}
