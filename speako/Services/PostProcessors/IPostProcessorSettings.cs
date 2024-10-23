using System.ComponentModel;

namespace speako.Services.PostProcessors
{
  public interface IPostProcessorSettings
  {
    public string DisplayName { get; }

    public IPostProcessorControl GetSettingsControl();

    public IPostProcessor GetPostProcessor();

    string GUID { get; set; }
    string Name { get; set; }

    public bool ProcessAfter { get; set; }

    string CastType { get; }

    void Init();

    public event PropertyChangedEventHandler PropertyChanged;

    public IPostProcessorSettings Duplicate();
  }
}
