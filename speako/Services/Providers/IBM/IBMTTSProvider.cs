using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.TextToSpeech.v1;
using speako.Services.Auth;
using speako.Services.Providers;
using speako.Services.Providers.AWS;
using speako.Services.Providers.Google;
using speako.Services.ProviderSettings;
using speako.Services.VoiceSettings;
using speako.Settings;

namespace speako.Services.Providers.IBM
{
  public class IBMTTSProvider : ITTSProvider
  {
    public string Name => "IBM";

    private string apiKey;
    private string serviceUrl;

    public override string ToString()
    {
      return Name;
    }

    public IBMTTSProvider()
    {
    }

    public async Task<Stream> GetSpeechFromTextAsync(string text, CancellationToken token)
    {

      // Create IAM Authenticator
      IamAuthenticator authenticator = new IamAuthenticator(apikey: apiKey);

      // Create Text to Speech service
      var textToSpeech = new TextToSpeechService(authenticator);
      textToSpeech.SetServiceUrl(serviceUrl);

      // Create a synthesize request
      var result = textToSpeech.Synthesize(text, "audio/mpeg", "en-US_AllisonV3Voice");

      return result.Result;
    }

    public async Task<IEnumerable<IVoice>> GetVoicesAsync(CancellationToken token)
    {
      IamAuthenticator authenticator = new IamAuthenticator(apikey: apiKey);

      var textToSpeech = new TextToSpeechService(authenticator);
      var response = textToSpeech.ListVoices();

      return response.Result._Voices.Select(response => new IBMVoice(response)).ToList();
    }

    public void OpenSettings()
    {
      //throw new NotImplementedException();
    }

    public void LoadSettings(ConfiguredProvider cp)
    {
      //throw new NotImplementedException();
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



