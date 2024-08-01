using System.Net.Http.Headers;

using Newtonsoft.Json.Linq;

namespace speako.Services.Providers.ElevenLabs
{
    public class ElevenLabsTTSProvider : ITTSProvider
    {
        private static readonly HttpClient client = createHttpClient();

        private static HttpClient createHttpClient()
        {
            //Load JSON file from config/ElevenLabs.json and key the key api_key
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
            throw new NotImplementedException();
        }
    }

}
