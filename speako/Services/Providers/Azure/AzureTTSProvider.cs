using Microsoft.CognitiveServices.Speech;
using speako.Common;
using speako.Services.Auth;
using speako.Services.Providers;
using speako.Services.Providers.AWS;
using speako.Services.ProviderSettings;
using speako.Services.VoiceProfiles;
using speako.Settings;
using System.Drawing;

namespace speako.Services.Providers.Azure
{
  public class AzureTTSProvider : ITTSProvider
  {
    public string Name => "Azure";

    public override string ToString()
    {
      return Name;
    }

    private string subscriptionKey;
    private string region;

    public AzureTTSProvider()
    {
    }

    public async Task<Stream> GetSpeechFromTextAsync(string text, CancellationToken token)
    {
      var config = SpeechConfig.FromSubscription(subscriptionKey, region);
      using var synthesizer = new SpeechSynthesizer(config);

      var result = await synthesizer.SpeakTextAsync(text);

      // Check the result
      if (result.Reason == ResultReason.SynthesizingAudioCompleted)
      {
        var ms = new MemoryStream(result.AudioData);
        ms.Position = 0;
        return ms;
      }
      else if (result.Reason == ResultReason.Canceled)
      {
        var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
        throw new SpeakoException($"Speech synthesis canceled: {cancellation.Reason}");
      }
      else
      {
        throw new SpeakoException($"Speech synthesis failed: {result.Reason}");
      }
    }

    public async Task<IEnumerable<IVoice>> GetVoicesAsync(CancellationToken token)
    {
      var config = SpeechConfig.FromSubscription(subscriptionKey, region);
      using var synthesizer = new SpeechSynthesizer(config);

      var voices = await synthesizer.GetVoicesAsync();

      return voices.Voices.Select(voice => new AzureVoice(voice)).ToList();
    }

    public IProviderSettingsControl SettingsControl()
    {
      throw new NotImplementedException();
    }

    public void LoadSettings(IAuthSettings settingsObject)
    {
      throw new NotImplementedException();
    }

    public Task<Stream> GetSpeechFromTextAsync(string text, VoiceProfiles.VoiceProfile profile, CancellationToken token)
    {
      throw new NotImplementedException();
    }

    public VoiceProfiles.VoiceProfile DefaultVoiceProfile()
    {
      throw new NotImplementedException();
    }

    public async Task<bool> CanConnectToTTSClient()
    {
      throw new NotImplementedException();
    }

    public async Task<object> CreateClient()
    {
      throw new NotImplementedException();
    }

    public Task<bool> Configure(IAuthSettings settingsObject)
    {
      throw new NotImplementedException();
    }
  }
}



