using NAudio.Wave;

namespace speako.Common
{
    class AudioDevice
    {
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
            throw new Exception("VB-Audio Virtual Cable not found.");
        }


    }
}
