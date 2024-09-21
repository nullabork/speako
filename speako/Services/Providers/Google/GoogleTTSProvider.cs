﻿using Google.Cloud.TextToSpeech.V1;
using speako.Services.ProviderSettings;
using speako.Settings;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using speako.Services.Auth;
using speako.Common;
using speako.Services.VoiceSettings;


namespace speako.Services.Providers.Google
{
  public class GoogleTTSProvider : ITTSProvider
  {

    private ConfiguredProvider _config;

    public string Name => "Google";
    private GoogleAuthSettings _settings;
    private TextToSpeechClient _client;

    public override string ToString()
    {
      return Name;
    }

    public void LoadSettings(IAuthSettings providerSettings)
    {
      _settings = (GoogleAuthSettings)providerSettings;
      _client = (TextToSpeechClient)CreateClient(_settings);
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


    public IProviderSettingsControl SettingsControl()
    {
      return new GoogleSettingsControl();
    }

    public VoiceProfile DefaultVoiceProfile()
    {
      return new VoiceProfile
      {
        Pitch = RangeConverter.ConvertRange<int>(0, -20, 20, 0, 100),
        Volume = RangeConverter.ConvertRange<int>(16, -96, 16, 0, 100),
        Speed = (int)RangeConverter.ConvertRange<double>(1.0, 0.25, 4, 0, 100)
      };
    }

    public async Task<Stream> GetSpeechFromTextAsync(string text, VoiceProfile profile, CancellationToken token)
    {

      // Define the input text to be synthesized
      SynthesisInput input = new SynthesisInput
      {
        Text = text
      };

      VoiceSelectionParams voice = new VoiceSelectionParams
      {
        Name = profile.VoiceID ?? "en-US-Wavenet-D",
        LanguageCode = "en-US",

      };

      

      // Specify the type of audio file to return
      AudioConfig config = new AudioConfig
      {
        AudioEncoding = AudioEncoding.Mp3,
        SampleRateHertz = 44100,
        Pitch = RangeConverter.ConvertRange<double>(Convert.ToDouble(profile.Pitch), 0,100, -20,20),
        SpeakingRate = RangeConverter.ConvertRange<double>(Convert.ToDouble(profile.Speed), 0, 100, 0.25, 4),
        VolumeGainDb = RangeConverter.ConvertRange<double>(Convert.ToDouble(profile.Volume), 0, 100, -96, 16),
      };


      // Perform the Text-to-Speech request
      SynthesizeSpeechResponse response = await _client.SynthesizeSpeechAsync(input, voice, config, token);

      var ms = new MemoryStream();
      response.AudioContent.WriteTo(ms);
      ms.Position = 0;

      return ms;
    }

    public async Task<IEnumerable<IVoice>> GetVoicesAsync(CancellationToken token)
    {
      if(_client == null) return Enumerable.Empty<IVoice>();

      var request = new ListVoicesRequest();

      var voices = await _client.ListVoicesAsync(request, token);
      return voices.Voices.Select(v => new GoogleVoice(v));
    }
  }
}




