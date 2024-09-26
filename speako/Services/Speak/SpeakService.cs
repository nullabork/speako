using Accord.DirectSound;
using Amazon;
using NAudio.Wave;
using speako.Common;
using speako.Services.Audio;
using speako.Services.Providers;
using speako.Services.VoiceSettings;
using speako.Settings;

namespace speako.Services.Speak
{
  public class SpeakService : ISpeakService, IAsyncDisposable
  {
    private readonly IEnumerable<ITTSProvider> _providers;
    private readonly VoiceQueue _voiceQueue;
    private readonly IAudioService _audio;
    private readonly Preferences _preferences;

    public SpeakService(IEnumerable<ITTSProvider> providers, VoiceQueue queue, IAudioService audio, Preferences preferences)
    {
      _providers = providers;
      _voiceQueue = queue;
      _audio = audio;
      _preferences = preferences;
    }

    public ValueTask DisposeAsync() => _voiceQueue.DisposeAsync();

    public async Task SpeakText(ITTSProvider ttsProvider, VoiceProfile profile, string text)
    {
      var deviceId = new Guid(profile.AudioDeviceGUID);
      var waveOut = _audio.GetDirectSoundOut(deviceId);

      var deviceId2 = new Guid(_preferences.AudioDeviceGUID);
      var waveOut2 = _audio.GetDirectSoundOut(deviceId2);

      //using var stream = await ttsProvider.GetSpeechFromTextAsync(text, profile, default);
      ////get device with accord
      //var devices = new AudioDeviceCollection(AudioDeviceCategory.Output).ToList();
      //int selectedIndex = 1;
      //var selectedDevice = devices[selectedIndex];


      //if (stream.CanSeek)
      //{
      //  stream.Position = 0;
      //}

      ////convert from mp3 to waveStream
      //MemoryStream copy = new MemoryStream();
      //stream.CopyTo(copy);
      //if (stream.CanSeek)
      //{
      //  copy.Position = 0;
      //  //stream.Position = 0;
      //}


      //using var mp3Reader = new Mp3FileReader(stream);

      //using var mp3Reader2 = new Mp3FileReader(copy);

      ////select device via NAudio guid and setup with stream rather
      //var waveOut = new DirectSoundOut(deviceId);
      //var waveOut2 = new DirectSoundOut(deviceId2);
      //waveOut.Init(mp3Reader);
      //waveOut2.Init(mp3Reader2);
      ////need to await the end of the audio playing
      //TaskCompletionSource<bool> playbackCompleted = new TaskCompletionSource<bool>();
      //waveOut.PlaybackStopped += (sender, e) => playbackCompleted.SetResult(true);

      //waveOut.Play();
      //waveOut2.Play();



      var webTask = Task.Run(async () =>
      {
        return await ttsProvider.GetSpeechFromTextAsync(text, profile, default);
      });

      await _voiceQueue.Enqueue(async (token) =>
      {
        //Wait for the webrequest to complete
        var stream1 = await webTask;
        MemoryStream stream2 = new MemoryStream();

        stream1.Position = 0;
        stream1.CopyTo(stream2);
        stream1.Position = 0;
        stream2.Position = 0;

        var wave1 = new Mp3FileReader(stream1);
        var wave2 = new Mp3FileReader(stream2);

        //convert from mp3 to waveStream
        waveOut.CompletionSource = new TaskCompletionSource<bool>();

        waveOut.DirectSound?.Init(wave1);
        waveOut.DirectSound?.Play();

        waveOut2.DirectSound?.Init(wave2);
        waveOut2.DirectSound?.Play();

        await waveOut.CompletionSource.Task;
        stream1.Dispose();
        stream2.Dispose();
      });
    }

  }
}
