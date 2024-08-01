using Google.Cloud.TextToSpeech.V1;

namespace speako.Services.Providers.Google
{
  public class GoogleTTSProvider : ITTSProvider
  {

    public string Name => "Google";

    public GoogleTTSProvider()
    {

    }

    public async Task<Stream> GetSpeechFromTextAsync(string text, CancellationToken token)
    {

      Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "config\\GoogleAuth.json");

      // Initialize the Text-to-Speech client
      TextToSpeechClient client = TextToSpeechClient.Create();

      // Define the input text to be synthesized
      SynthesisInput input = new SynthesisInput
      {
        Text = text
      };

      VoiceSelectionParams voice = new VoiceSelectionParams
      {
        Name = "en-US-Wavenet-D",
        LanguageCode = "en-US"
      };

      // Specify the type of audio file to return
      AudioConfig config = new AudioConfig
      {
        AudioEncoding = AudioEncoding.Mp3
      };


      // Perform the Text-to-Speech request
      SynthesizeSpeechResponse response = await client.SynthesizeSpeechAsync(input, voice, config, token);

      var ms = new MemoryStream();
      response.AudioContent.WriteTo(ms);
      ms.Position = 0;

      return ms;
    }

    public async Task<IEnumerable<IVoice>> GetVoicesAsync(CancellationToken token)
    {
      TextToSpeechClient client = TextToSpeechClient.Create();

      var request = new ListVoicesRequest();

      var voices = await client.ListVoicesAsync(request, token);
      return voices.Voices.Select(v => new GoogleVoice(v));
    }
  }
}



