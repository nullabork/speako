using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.TextToSpeech.v1;

namespace speako.Features.Speak.Providers
{
    public class IBMTTSProvider : ITTSProvider
    {
        private string apiKey;
        private string serviceUrl;

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
    }
}



