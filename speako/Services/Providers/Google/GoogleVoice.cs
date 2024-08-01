using Google.Cloud.TextToSpeech.V1;

namespace speako.Services.Providers.Google
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