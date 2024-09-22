using speako.Services.Auth;
using speako.Services.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Services.ProviderSettings
{
  public interface IProviderSettingsControl
  {
    public event EventHandler<IAuthSettings> Saved;
    public event EventHandler<IAuthSettings> Cancel;

    public void SetAuthSettings(IAuthSettings settings);
    public IAuthSettings GetAuthSettings();

    public bool SaveOnClosing();
  }
}
