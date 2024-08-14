using speako.Services.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Settings
{
  public interface IConfig
  {
    string Name { get; set; }

    string ProviderName { get; set; }

    ITTSProvider Provider { get; set; }


    string uuid { get; set; }
  }
}
