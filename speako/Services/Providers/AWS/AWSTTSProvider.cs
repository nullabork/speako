﻿using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using speako.Settings;

namespace speako.Services.Providers.AWS
{
  public class AWSTTSProvider : ITTSProvider
  {
    public string Name => "Amazon";

    public override string ToString()
    {
      return Name;
    }

    public async Task<Stream> GetSpeechFromTextAsync(string text, CancellationToken token)
    {
      var client = new AmazonPollyClient(RegionEndpoint.USEast1);
      var request = new SynthesizeSpeechRequest
      {
        OutputFormat = OutputFormat.Mp3,
        Text = text,
        VoiceId = VoiceId.Joanna
      };

      var response = await client.SynthesizeSpeechAsync(request, token);
      var stream = new MemoryStream();
      await response.AudioStream.CopyToAsync(stream);
      stream.Position = 0;

      return stream;
    }

    public async Task<IEnumerable<IVoice>> GetVoicesAsync(CancellationToken token)
    {
      var client = new AmazonPollyClient(RegionEndpoint.USEast1);
      var request = new DescribeVoicesRequest();

      var response = await client.DescribeVoicesAsync(request, token);

      var voices = new List<IVoice>();
      foreach (var voice in response.Voices)
      {
        voices.Add(new AWSVoice(voice));
      }
      return voices;
    }

    public void OpenSettings()
    {
      //throw new NotImplementedException();
    }

    public void LoadSettings(ConfiguredProvider cp)
    {
      //throw new NotImplementedException();
    }
  }
}



