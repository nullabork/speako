﻿using speako.Services.Auth;
using speako.Services.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Services.Providers
{
  public interface IProviderSettingsControl
  {
    public event EventHandler<IAuthSettings> Saved;
    public event EventHandler<IAuthSettings> Cancel;

    public void Configure(IAuthSettings settings);

    public IAuthSettings SaveOnClosing();
  }
}
