using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace speako.Services.Speak
{
  public class MessageSession : JsonSerializable<MessageSession>, IMessageSession
  {
    public ObservableCollection<TextMessage> Messages { get; set; } = new ObservableCollection<TextMessage>();

    public DateTime DateTime { get; set; }

    public string Name { get; set; }

    public string CastType => GetType().ToString();

    public string AsString()
    {
      return $"Session: {DateTime}\n{ string.Join("\n", Messages.Select(message => message.AsString()).ToArray()) }";
    }
  }
}
