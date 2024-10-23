using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Services.PreProcessors.TextReplacer
{
  public class MessageReplacement: INotifyPropertyChanged
  {

    public string GUID { get; set; } = Guid.NewGuid().ToString();

    public string From {  get; set; }

    public string VoiceText { get; set; }

    public string MessageText { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string AfterReplace(string message)
    {
      return message.Replace(From, VoiceText);
    }

    public string BeforeReplace(string message)
    {
      return message.Replace(From, MessageText);
    }

    public string CastType => this.GetType().ToString();
  }
}
