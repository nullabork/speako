using speako.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Services.PostProcessors
{
  public interface IPostProcessor
  {
    public Task Process(string input);

    public Task Configure(IPostProcessorSettings settings);

  }
}
