using Accord.DirectSound;

using FuzzySharp;

using NAudio.CoreAudioApi;

using System.Collections.Generic;

namespace speako.Common
{
  public class AudioDevice
  {
    public string DeviceName { get; set; }
    public string DeviceGuid { get; set; }

    public override string ToString() => DeviceName;
  }

  public class AudioDevicesSingleton
  {
    private static readonly Lazy<AudioDevicesSingleton> _instance = new Lazy<AudioDevicesSingleton>(() => new AudioDevicesSingleton());
    public static AudioDevicesSingleton Instance => _instance.Value;

    public Dictionary<string, AudioDevice> AudioDevices { get; private set; }

    private AudioDevicesSingleton()
    {
      AudioDevices = InitializeDevices();
    }

    private Dictionary<string, AudioDevice> InitializeDevices()
    {
      var devices = new Dictionary<string, AudioDevice>();

      var found = new AudioDeviceCollection(AudioDeviceCategory.Output).ToList();
      foreach (var device in found)
      {
        var guid = device.Guid.ToString();

        devices.Add(guid, new AudioDevice
        {
          DeviceName = device.Description,
          DeviceGuid = guid
        });
      }

      return devices;
    }
  }
}

