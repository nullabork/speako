using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Services.PostProcessors.DiscordWebHook
{
  public class DiscordChannelWebHook: INotifyPropertyChanged
  {
    
    public string GUID { get; set; } = Guid.NewGuid().ToString();

    public string Name { get; set; }

    public string WebHookURL { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
  }
}
