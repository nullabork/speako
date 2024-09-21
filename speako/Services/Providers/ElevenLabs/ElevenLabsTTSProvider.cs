using System.Net.Http.Headers;

using Newtonsoft.Json.Linq;
using speako.Services.Auth;
using speako.Services.Providers.AWS;
using speako.Services.Providers.Azure;
using speako.Services.ProviderSettings;
using speako.Services.VoiceSettings;
using speako.Settings;

namespace speako.Services.Providers.ElevenLabs
{
  public class ElevenLabsTTSProvider : ITTSProvider
  {
    public string Name => "Eleven Labs";

    public override string ToString()
    {
      return Name;
    }


    private static readonly HttpClient client = createHttpClient();

    private static HttpClient createHttpClient()
    {
      //ConfigureVoice JSON file from config/ElevenLabs.json and key the key api_key
      var authFilePath = Path.Combine("config", "ElevenAuth.json");
      var json = JObject.Parse(File.ReadAllText(authFilePath));
      var apiKey = json["api_key"]?.ToString();

      if (string.IsNullOrEmpty(apiKey))
      {
        throw new Exception($"API key not found in {authFilePath}");
      }

      var client = new HttpClient();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("audio/mpeg"));
      client.DefaultRequestHeaders.Add("xi-api-key", apiKey);
      return client;
    }

    public async Task<Stream> GetSpeechFromTextAsync(string text, CancellationToken token)
    {
      string voiceId = "EXAVITQu4vr4xnSDxMaL"; // Replace with the desired voice ID
      string url = $"https://api.elevenlabs.io/v1/text-to-speech/{voiceId}/stream";

      var requestData = new
      {
        text,
        model_id = "eleven_monolingual_v1",
        voice_settings = new
        {
          stability = 0.5,
          similarity_boost = 0.5
        }
      };

      var response = await client.PostAsJsonAsync(url, requestData, token);
      response.EnsureSuccessStatusCode();

      var stream = await response.Content.ReadAsStreamAsync(token);
      return stream;
    }

    public async Task<IEnumerable<IVoice>> GetVoicesAsync(CancellationToken token)
    {
      return new List<IVoice>
            {
                new ElevenLabsVoice("EXAVITQu4vr4xnSDxMaL", "Eleven Labs Voice")
            };
    }

    public IProviderSettingsControl SettingsControl()
    {
      throw new NotImplementedException();
    }

    public void LoadSettings(IAuthSettings settingsObject)
    {
      throw new NotImplementedException();
    }

    public Task<Stream> GetSpeechFromTextAsync(string text, VoiceProfile profile, CancellationToken token)
    {
      throw new NotImplementedException();
    }

    public VoiceProfile DefaultVoiceProfile()
    {
      throw new NotImplementedException();
    }

    public async Task<bool> CanConnectToTTSClient()
    {
      throw new NotImplementedException();
    }

    public object CreateClient(IAuthSettings authSettings)
    {
      throw new NotImplementedException();
    }
  }

}
