using Newtonsoft.Json;
using speako.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Services.VoiceSettings
{
  public class VoiceProfile : INotifyPropertyChanged
  {

    public event PropertyChangedEventHandler PropertyChanged;

    public VoiceProfile() {
      CastType = this.GetType().ToString();
    }

    public string GUID { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string ConfiguredProviderGUID { get; set; }

    public string VoiceID { get; set; }

    public string AudioDeviceGUID { get; set; }

    public int Volume { get; set; }

    public int Speed { get; set; }

    public int Pitch { get; set; }

    public string DeviceName
    {
      get; set;
    }

    public string TTSTestSentence
    { 
      get; set;
    }

    public string CastType { get; set; } 
  }
}
