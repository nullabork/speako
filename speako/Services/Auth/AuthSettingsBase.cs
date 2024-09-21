using speako.Services.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace speako.Services.Auth
{
  public abstract class AuthSettingsBase
  {
    public abstract string Name { get; set; }
    public abstract ITTSProvider Provider { get; set; }

    public string GetNiceName
    {
      get { return Name + (Provider != null ? " - " + Provider.Name : ""); }
    }
  }
}
