using Accord.DirectSound;
using Amazon.Runtime.Internal.Util;
using NAudio.Wave;
using speako.Common;
using speako.Services.Providers;
using speako.Services.Providers.Google;
using speako.Services.VoiceSettings;
using System.Diagnostics;

namespace speako.Services.Speak
{
  public class SpeakService : ISpeakService
  {
    private readonly IEnumerable<ITTSProvider> _providers;
    private VoiceQueue _voiceQueue;
    public SpeakService(IEnumerable<ITTSProvider> providers)
    {
      _providers = providers;
      _voiceQueue = new VoiceQueue();
    }

    public async Task SpeakText(ITTSProvider ttsProvider, VoiceProfile profile, string text)
    {
      _voiceQueue.Enqueue(async () =>
      {

        //generate text
        using var stream = await ttsProvider.GetSpeechFromTextAsync(text, profile, default);

        //get device with accord
        var devices = new AudioDeviceCollection(AudioDeviceCategory.Output).ToList();
        int selectedIndex = 1;
        var selectedDevice = devices[selectedIndex];

        //convert from mp3 to waveStream
        using var mp3Reader = new Mp3FileReader(stream);
        
        //select device via NAudio guid and setup with stream rather
        var waveOut = new DirectSoundOut(new Guid(profile.AudioDeviceGUID));
        waveOut.Init(mp3Reader);

        //need to await the end of the audio playing
        TaskCompletionSource<bool> playbackCompleted = new TaskCompletionSource<bool>();
        waveOut.PlaybackStopped += (sender, e) => playbackCompleted.SetResult(true);
        

        waveOut.Play();
        await playbackCompleted.Task;
      });
    }

  }
}
