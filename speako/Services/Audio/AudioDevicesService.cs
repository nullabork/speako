using Accord.DirectSound;

namespace speako.Services.Audio
{
  public class AudioDevicesService
  {
    public Dictionary<string, AudioDevice> AudioDevices { get; private set; }

    public AudioDevicesService()
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

