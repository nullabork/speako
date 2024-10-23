

using speako.Services.Auth;

namespace speako.Services.PreProcessors
{
  public interface IPreProcessorControl
  {
    public Task Configure(IPreProcessorSettings settings);

    public event EventHandler<IPreProcessorSettings> Saved;
    public event EventHandler<IPreProcessorSettings> Cancel;
  }
}
