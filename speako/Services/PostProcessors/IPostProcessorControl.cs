namespace speako.Services.PostProcessors
{
  public interface IPostProcessorControl
  {
    public Task Configure(IPostProcessorSettings settings);

    public event EventHandler<IPostProcessorSettings> Saved;
    public event EventHandler<IPostProcessorSettings> Cancel;
  }
}
