
//hate this
namespace speako.Services.PostProcessors
{
  public class PostProcessorItem
  {
    public string ProcessorName { get; set; }
    public string ProcessorGuid { get; set; }
    public override string ToString() => ProcessorName;
    public string CastType => this.GetType().ToString();
  }
}
