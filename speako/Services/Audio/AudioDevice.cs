using FuzzySharp;

using NAudio.CoreAudioApi;
using System.Collections.Generic;

namespace speako.Services.Audio
{
  public class AudioDevice: IAudioDevice
  {

    public string DeviceName { get; set; }
    public string DeviceGuid { get; set; }
    public override string ToString() => DeviceName;

    public bool MicMerge = false;
    public string CastType => this.GetType().ToString();
  }
}

