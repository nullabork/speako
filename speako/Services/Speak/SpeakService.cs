using NAudio.Wave;
using speako.Common;
using speako.Services.Audio;
using speako.Services.PostProcessors;
using speako.Services.PreProcessors;
using speako.Services.Providers;
using speako.Services.VoiceProfiles;
using speako.Settings;

namespace speako.Services.Speak
{
  public class SpeakService : ISpeakService, IAsyncDisposable
  {
    private readonly IEnumerable<ITTSProvider> _providers;
    private readonly VoiceQueue _voiceQueue;
    private readonly IAudioService _audio;
    private readonly Preferences _preferences;
    private readonly ApplicationSettings _applicationSettings;

    public SpeakService(IEnumerable<ITTSProvider> providers, VoiceQueue queue, ApplicationSettings appSettings, IAudioService audio, Preferences preferences)
    {
      _providers = providers;
      _voiceQueue = queue;
      _audio = audio;
      _preferences = preferences;
      _applicationSettings = appSettings;
    }
    private ITTSProvider GetTTSProvider(VoiceProfile profile)
    {
      return _applicationSettings?.ProviderSettingsLookup?
          .GetValueOrDefault(profile.ConfiguredProviderGUID)?
          .GetProvider();
    }

    private IEnumerable<IPreProcessor> GetPreProcessors(VoiceProfile profile)
    {
      return profile.PreProcessors
          .Select(pp => _applicationSettings?.PreProcessorLookup
              .GetValueOrDefault(pp.ProcessorGuid)?
              .GetPreProcessor())
          .Where(pp => pp != null);
    }

    private PostProcessors GetPostProcessors(VoiceProfile profile)
    {
      var postProcessorInfos = profile.PostProcessors
          .Select(ppi => _applicationSettings?.PostProcessorLookup
              .GetValueOrDefault(ppi.ProcessorGuid))
          .Where(ppi => ppi != null);

      var syncProcessors = postProcessorInfos
          .Where(ppi => !ppi.ProcessAfter)
          .Select(ppi => ppi.GetPostProcessor())
          .ToList();

      var asyncProcessors = postProcessorInfos
          .Where(ppi => ppi.ProcessAfter)
          .Select(ppi => ppi.GetPostProcessor())
          .ToList();

      return new PostProcessors
      {
        SyncProcessors = syncProcessors,
        AsyncProcessors = asyncProcessors
      };
    }
    private IEnumerable<DeviceHandler> GetDeviceHandlers(VoiceProfile profile)
    {
      return profile.AudioDevices
          .Select(ad => _audio.GetDirectSoundOut(new Guid(ad.DeviceGuid)));

    }
    public ValueTask DisposeAsync() => _voiceQueue.DisposeAsync();

    public async Task SpeakText(string message, VoiceProfile profile)
    {
      var pText = new PText
      {
        voice = message,
        message = message,
      };

      var ttsProvider = GetTTSProvider(profile);
      if (ttsProvider == null)
        return;

      var preProcessors = GetPreProcessors(profile);
      var postProcessors = GetPostProcessors(profile);
      var deviceHandlers = GetDeviceHandlers(profile);

      foreach (var processor in preProcessors)
      {
        if (processor == null) continue;
        pText = await processor.Process(profile, ObjectUtils.Clone(pText));
      }

      var webTask = Task.Run(async () =>
      {
        return await ttsProvider.GetSpeechFromTextAsync(pText.voice, profile, default);
      });

      await _voiceQueue.Enqueue(async (token) =>
      {
        var mainStream = await webTask;
        var streams = new List<Stream>([mainStream]);

        for (var i = 0; i < deviceHandlers.Count(); i++)
        {
          var waveOut = deviceHandlers.ElementAt(i);

          //blank stream to copy main stream to for the device playing as we may need multiple copies and cant play from the mainStream
          var deviceStream = new MemoryStream();

          //Add stream to known streams so we can clean up after
          streams.Add(deviceStream);
          //set main stream to 0 so we can copy to device stream
          mainStream.Position = 0;
          //do the copy
          mainStream.CopyTo(deviceStream);
          //make sure the copy is at position 0
          deviceStream.Position = 0;

          //TODO make sure we can handle different stream forms
          var wave1 = new Mp3FileReader(deviceStream);

          
          waveOut.CompletionSource = new TaskCompletionSource<bool>();
          waveOut.DirectSound?.Init(wave1);
          waveOut.DirectSound?.Play();
        }

        postProcessors.SyncProcessors.ForEach(pp => pp.Process(profile, pText));

        await Task.WhenAll(deviceHandlers.Select(dh => dh.CompletionSource.Task));
        await Task.WhenAll(postProcessors.AsyncProcessors.Select(pp => pp.Process(profile, pText)));

        streams.ForEach(stream => stream.Dispose());

        deviceHandlers.ToList().ForEach(dh =>
        {
          dh.DirectSound?.Stop();
          dh.DirectSound?.Dispose();
        });
      });
    }

    private class PostProcessors
    {
      public List<IPostProcessor> SyncProcessors { get; set; }
      public List<IPostProcessor> AsyncProcessors { get; set; }
    }
  }
}
