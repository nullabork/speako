using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace speako.Services.Speak
{
  public class TextMessage
  {
    public string Message { get; set; }
    public string VoiceProfileName { get; set; }

    public DateTime DateTime { get; set; } = DateTime.Now;

    public string CastType => GetType().ToString();


    public string AsString()
    {
      return $"{DateTime} - {VoiceProfileName} : {Message}";
    }
  }
}
