
using IBM.Watson.TextToSpeech.v1.Model;

namespace speako.Providers
{
  public class IBMVoice : IVoice
  {
    public IBMVoice(Voice voice)
    {
      Name = voice.Name;
      Language = voice.Language;
    }

    public string Name { get; }
    public string Language { get; }
  }
}



