using speako.Services.Speak;
using speako.Services.VoiceProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Services.PostProcessors
{
  public interface IPostProcessor
  {
    public Task<PText> Process(VoiceProfile vp, PText pText);

    public Task<bool> Configure(IPostProcessorSettings settings);

  }
}
