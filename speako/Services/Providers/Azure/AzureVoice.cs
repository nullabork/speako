using Microsoft.CognitiveServices.Speech;

namespace speako.Services.Providers.Azure
{
  public class AzureVoice : IVoice
  {
    public AzureVoice(VoiceInfo voice)
    {
      Name = voice.Name;
      Language = voice.Locale;
    }

    public string Name { get; }
    public string Language { get; }

    public string ConfuredProviderUUID => throw new NotImplementedException();
  }
}



