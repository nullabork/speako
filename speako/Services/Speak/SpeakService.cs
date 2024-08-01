using NAudio.Wave;

using speako.Features.Speak.Providers;
using speako.Google;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Features.Speak
{
  public class SpeakService : ISpeakService
  {
    private readonly IEnumerable<ITTSProvider> _providers;

    public SpeakService(IEnumerable<ITTSProvider> providers)
    {
      _providers = providers;
    }

    public async Task SpeakText(string text)
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
