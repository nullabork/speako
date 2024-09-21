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

    string ProviderName { get; set; }

    ITTSProvider Provider { get; set; }


    string GUID { get; set; }
  }
}
