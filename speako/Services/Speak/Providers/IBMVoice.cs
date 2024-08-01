
using IBM.Watson.TextToSpeech.v1.Model;

namespace speako.Features.Speak.Providers
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



