using Google.Cloud.TextToSpeech.V1;
using speako.Settings;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using speako.Services.Auth;
using speako.Common;
using speako.Services.VoiceProfiles;

namespace speako.Services.Providers.Google
{

  public class GoogleTTSProvider : ITTSProvider
  {
    public string Name => "Google";
    private GoogleAuthSettings _settings;
    private TextToSpeechClient _client;
    private static List<IVoice> _voiceCache;

    private readonly ProviderAttribute _pitchAttr = new ProviderAttribute { Default = 0, Min = -20, Max = 20 };
    private readonly ProviderAttribute _volumeAttr = new ProviderAttribute { Default = 16, Min = -96, Max = 16 };
    private readonly ProviderAttribute _speedAttr = new ProviderAttribute { Default = 1.0, Min = 0.25, Max = 4.0 };

    public override string ToString()
    {
      return Name;
    }

    public async Task<bool> Configure(IAuthSettings providerSettings)
    {
      var task = new TaskCompletionSource<bool>();

      _settings = (GoogleAuthSettings)providerSettings;
      _client = (TextToSpeechClient)CreateClient(_settings);
      task.SetResult(true);

      return await task.Task;
    }

    public object CreateClient(IAuthSettings providerSettings)
    {
      try
      {
        var json = JsonConvert.SerializeObject(_settings, FilterKeys.Exclude(["Name", "CastType", "GUID"]));
        json = json.Replace("\\\\n", "\\n");
        var credentials = GoogleCredential.FromJson(json).CreateScoped(TextToSpeechClient.DefaultScopes);

        return new TextToSpeechClientBuilder
        {
          Credential = credentials
        }.Build();
      }
      catch (Exception ex)
      {

      }

      return null;
    }


    public async Task<bool> CanConnectToTTSClient()
    {
      //if _settings look at least somewhat correct
      if (_settings == null || !_settings.IsConfigured())  return false;

      var tempClient = (TextToSpeechClient)CreateClient(_settings);

      //client still null
      if (tempClient == null) return false;

      var request = new ListVoicesRequest();
      var response = await tempClient.ListVoicesAsync(request);

      //no voices
      if (response == null || response?.Voices?.Count() == 0) return false;
      return true;
    }

    public VoiceProfiles.VoiceProfile DefaultVoiceProfile()
    {
      return new VoiceProfiles.VoiceProfile
      {
        Pitch = _pitchAttr.GetDefault(),
        Volume = _volumeAttr.GetDefault(),
        Speed = _speedAttr.GetDefault(),
      };
    }

    public async Task<Stream> GetSpeechFromTextAsync(string text, VoiceProfiles.VoiceProfile profile, CancellationToken token)
    {
      SynthesisInput input = new SynthesisInput
      {
        Text = text
      };

      VoiceSelectionParams voice = new VoiceSelectionParams
      {
        Name = profile.VoiceID ?? "en-US-Wavenet-D",
        LanguageCode = "en-US",

      };

      AudioConfig config = new AudioConfig
      {
        AudioEncoding = AudioEncoding.Mp3,
        SampleRateHertz = 44100,
        Pitch = _pitchAttr.GetValue(profile.Pitch),
        SpeakingRate = _speedAttr.GetValue(profile.Speed),
        VolumeGainDb = _volumeAttr.GetValue(profile.Volume),
      };

      SynthesizeSpeechResponse response = await _client.SynthesizeSpeechAsync(input, voice, config, token);

      var ms = new MemoryStream();
      response.AudioContent.WriteTo(ms);
      ms.Position = 0;

      return ms;
    }

    public async Task<IEnumerable<IVoice>> GetVoicesAsync(CancellationToken token)
    {
      if (_client == null) return new List<IVoice> { };

      if (_voiceCache != null && _voiceCache.Count() > 0)
      {
        return _voiceCache;
      }

      var request = new ListVoicesRequest();
      var resp = await _client.ListVoicesAsync(request, token);

      _voiceCache = resp.Voices.Select(v => new GoogleVoice(v)).ToList<IVoice>();

      return _voiceCache;
    }
  }
}




