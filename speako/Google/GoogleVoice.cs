using Google.Cloud.TextToSpeech.V1;
using speako.Providers;

namespace speako.Google
{
    internal class GoogleVoice : IVoice
    {
        private Voice v;

        public GoogleVoice(Voice v)
        {
            this.v = v;
        }

        public string Name => v.Name;

        public string Language => v.LanguageCodes[0];
    }
}