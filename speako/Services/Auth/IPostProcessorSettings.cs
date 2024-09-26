using speako.Services.PostProcessors;
using speako.Services.Providers;
using System.ComponentModel;

namespace speako.Services.Auth
{
  public interface IPostProcessorSettings
  {
    string GUID { get; set; }
    string Name { get; set; }
    string CastType { get; }
    IPostProcessor Processor { get; set; }

    void Init();

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
