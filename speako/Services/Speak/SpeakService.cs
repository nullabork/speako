using Amazon;
using NAudio.Wave;
using speako.Common;
using speako.Services.Audio;
using speako.Services.Providers;
using speako.Services.VoiceSettings;

namespace speako.Services.Speak
{
  public class SpeakService : ISpeakService, IAsyncDisposable
  {
    private readonly IEnumerable<ITTSProvider> _providers;
    private readonly VoiceQueue _voiceQueue;
    private readonly IAudioService _audio;

    public SpeakService(IEnumerable<ITTSProvider> providers, VoiceQueue queue, IAudioService audio)
    {
      _providers = providers;
      _voiceQueue = queue;
      _audio = audio;
    }

    public ValueTask DisposeAsync() => _voiceQueue.DisposeAsync();

    public async Task SpeakText(ITTSProvider ttsProvider, VoiceProfile profile, string text)
    {
      var deviceId = new Guid(profile.AudioDeviceGUID);
      var waveOut = _audio.GetDirectSoundOut(deviceId); 
      Stream ms = null;

      var webTask = Task.Run(async () =>
      {
        ms = await ttsProvider.GetSpeechFromTextAsync(text, profile, default);
        return new Mp3FileReader(ms);
      });

      await _voiceQueue.Enqueue(async (token) =>
      {
        //Wait for the webrequest to complete
        var wave = await webTask;

        //convert from mp3 to waveStream
        waveOut.CompletionSource = new TaskCompletionSource<bool>();
        waveOut.DirectSound?.Init(wave);
        waveOut.DirectSound?.Play();
        await waveOut.CompletionSource.Task;
        ms.Dispose();
      });
    }

  }
}
