using NAudio.Wave;
using speako.Google;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
