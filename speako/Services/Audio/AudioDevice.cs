﻿using FuzzySharp;

using NAudio.CoreAudioApi;
using System.Collections.Generic;

namespace speako.Services.Audio
{
  public class AudioDevice: IAudioDevice
  {

    public AudioDevice()
    {
      //CastType = this.GetType().ToString();
    }

    public string DeviceName { get; set; }
    public string DeviceGuid { get; set; }
    public override string ToString() => DeviceName;
    public string CastType => this.GetType().ToString();
  }
}

