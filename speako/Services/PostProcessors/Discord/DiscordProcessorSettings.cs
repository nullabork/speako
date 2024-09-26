using speako.Services.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Services.PostProcessors.Discord
{
  class DiscordProcessorSettings : IPostProcessorSettings
  {
    //just unique id used for references
    public string GUID { get; set; } = Guid.NewGuid().ToString();

    //Name purely used for display in the UI
    public string Name { get; set; }

    //Actual config for Discord.net
    public string BotToken { get; set; }

    //Actual config for Discord.net
    public List<DiscordChannel> ChannelIds { get; set; } = new List<DiscordChannel>();


    //Cast type is used by the newtonSoft json so it knows what class to create when deserialising
    public string CastType => this.GetType().ToString();

    //Probably not the best way to get the processor
    public IPostProcessor Processor { get; set; }

    //setup the processor after we load it from json
    public void Init()
    {
      Processor = new DiscordProcessor();
    }

    //helps with the window controls
    public event PropertyChangedEventHandler PropertyChanged;
  }
}
