using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Services.VoiceSettings
{
  internal class ConfiguredVoice
  {
    public string Name { get; set; }
    public string ConfiguredProviderUUID { get; set; }

    public string VoiceID { get; set; }

    public string AudioDeviceProductGUID { get; set; }

    public int Volume { get; set; }

    public int Speed { get; set; }

    public int Pitch { get; set; }
  }
}
