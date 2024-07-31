using Microsoft.CognitiveServices.Speech;

namespace speako.Providers
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
  }
}



