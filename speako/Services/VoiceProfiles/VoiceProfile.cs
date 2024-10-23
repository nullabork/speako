using speako.Services.Audio;
using speako.Services.PostProcessors;
using speako.Services.PreProcessors;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace speako.Services.VoiceProfiles
{
  public class VoiceProfile : INotifyPropertyChanged
  {

    public event PropertyChangedEventHandler PropertyChanged;

    public string GUID { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string ConfiguredProviderGUID { get; set; }

    public string VoiceID { get; set; }

    public string AudioDeviceGUID { get; set; }

    public ObservableCollection<AudioDevice> AudioDevices { get; set; } = new ObservableCollection<AudioDevice>();

    public ObservableCollection<PostProcessorItem> PostProcessors { get; set; } = new ObservableCollection<PostProcessorItem>();

    public ObservableCollection<PreProcessorItem> PreProcessors { get; set; } = new ObservableCollection<PreProcessorItem>();

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

    public string CastType => this.GetType().ToString();
  }
}
