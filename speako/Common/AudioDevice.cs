using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace speako.Common
{
  class AudioDevice
  {

    public string ProductName { get; set; }
    public string ProductGuid { get; set; }

    // Override ToString to display the ProductName in the ListBox
    public override string ToString()
    {
      return ProductName;
    }


    public static int GetVBCableDeviceNumber()
    {
      for (int i = 0; i < WaveOut.DeviceCount; i++)
      {
        var capabilities = WaveOut.GetCapabilities(i);
        if (capabilities.ProductName.Contains("VB-Audio"))
        {
          return i;
        }
      }

      return -1;
    }




    public static List<AudioDevice> GetAudioDevices()
    {
      var devices = new List<AudioDevice>();
      var enumerator = new MMDeviceEnumerator();
      var waveOutDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

      // Create a dictionary for fast lookup with lowercase friendly names as keys
      var deviceLookup = new Dictionary<string, string>();

      foreach (var device in waveOutDevices)
      {
        //first 10 characters of the friendly name
        var key = device.FriendlyName.Substring(0, 30);
        if (!deviceLookup.ContainsKey(key)) {
          deviceLookup.Add(key, device.FriendlyName);
        }
      }

      for (int i = 0; i < WaveOut.DeviceCount; i++)
      {
        var capabilities = WaveOut.GetCapabilities(i);
        var compareKey = capabilities.ProductName.Substring(0, 30);

        var name = deviceLookup.ContainsKey(compareKey) ? deviceLookup[compareKey] : capabilities.ProductName;

        // Look for a match in the dictionary
        

        devices.Add(new AudioDevice
        {
          ProductName = name,
          ProductGuid = capabilities.ProductGuid.ToString()
        });
      }

      return devices;
    }

    public static int GetDeviceNumber(string productGuid)
    {
      for (int i = 0; i < WaveOut.DeviceCount; i++)
      {
        var capabilities = WaveOut.GetCapabilities(i);
        if (capabilities.ProductGuid.ToString() == productGuid)
        {
          return i;
        }
      }

      return -1;
    }

  }
}
