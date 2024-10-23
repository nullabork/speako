using speako.Services.PostProcessors;
using speako.Services.PreProcessors;
using System.ComponentModel;

namespace speako.Services.PreProcessors
{
  public interface IPreProcessorSettings
  {
    public string DisplayName { get; }

    public IPreProcessorControl GetSettingsControl();

    public IPreProcessor GetPreProcessor();

    string GUID { get; set; }
    string Name { get; set; }
    string CastType { get; }

    void Init();

    public event PropertyChangedEventHandler PropertyChanged;

    public IPreProcessorSettings Duplicate();
  }
}
