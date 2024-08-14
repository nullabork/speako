using Google.Cloud.TextToSpeech.V1;
using speako.Services.Providers.AWS;
using speako.Services.Providers.ElevenLabs;
using speako.Settings;

namespace speako.Services.Providers.Google
{
  public class GoogleTTSProvider : ITTSProvider
  {

    private ConfiguredProvider _config;

    public string Name => "Google";
    private GoogleAuthSettings _settings;
    public override string ToString()
    {
        return Name;
    }


    public void LoadSettings(ConfiguredProvider cp)
    {
      _config = cp;
      _settings = GoogleAuthSettings.Load(cp.uuid);
      Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", _settings.FilePath);
    }

    public void OpenSettings()
    {
      var settingsWindow = new SettingsWindow();
      var GoogleSettingsControl = new GoogleSettingsControl();

      //println _settings
      GoogleSettingsControl.SetDataContext(_settings);
      settingsWindow.settingsScroller.Content = GoogleSettingsControl;
      settingsWindow.ShowDialog();
    }

    public async Task<Stream> GetSpeechFromTextAsync(string text, CancellationToken token)
    {
      // Initialize the Text-to-Speech client
      TextToSpeechClient client = TextToSpeechClient.Create();

      // Define the input text to be synthesized
      SynthesisInput input = new SynthesisInput
      {
        Text = text
      };

      VoiceSelectionParams voice = new VoiceSelectionParams
      {
        Name = "en-US-Wavenet-D",
        LanguageCode = "en-US"
      };

      // Specify the type of audio file to return
      AudioConfig config = new AudioConfig
      {
        AudioEncoding = AudioEncoding.Mp3
      };


      // Perform the Text-to-Speech request
      SynthesizeSpeechResponse response = await client.SynthesizeSpeechAsync(input, voice, config, token);

      var ms = new MemoryStream();
      response.AudioContent.WriteTo(ms);
      ms.Position = 0;

      return ms;
    }

    public async Task<IEnumerable<IVoice>> GetVoicesAsync(CancellationToken token)
    {
      //return new List<IVoice>();
      //Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", _settings.FilePath);

      TextToSpeechClient client = TextToSpeechClient.Create();

      var request = new ListVoicesRequest();

      var voices = await client.ListVoicesAsync(request, token);
      return voices.Voices.Select(v => new GoogleVoice(v, _config));
    }
  }
}



