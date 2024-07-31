﻿using NAudio.Wave;
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

        public static async Task SpeakText(string text)
        {

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var ttsProvider = new GoogleTTSProvider();
            using var stream = await ttsProvider.GetSpeechFromTextAsync(text, default);

            //var voices = await ttsProvider.GetVoicesAsync();
            //foreach (var voice in voices)
            //{
            //    System.Diagnostics.Debug.WriteLine(voice.Name);
            //}

            System.Diagnostics.Debug.WriteLine("stream: " + stopwatch.ElapsedMilliseconds);

            using var waveOut = new WaveOutEvent { DeviceNumber = Common.AudioDevice.GetVBCableDeviceNumber() };
            using var mp3Reader = new Mp3FileReader(stream);

            waveOut.Init(mp3Reader);
            waveOut.Play();

            while (waveOut.PlaybackState == PlaybackState.Playing)
            {
                await Task.Delay(1000);
            }
        }
    }
}
