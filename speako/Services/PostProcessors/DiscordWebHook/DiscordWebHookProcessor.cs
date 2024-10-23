using System;
using System.Text;
using Newtonsoft.Json;
using speako.Services.Speak;
using speako.Services.VoiceProfiles;

namespace speako.Services.PostProcessors.DiscordWebHook
{
  class DiscordWebHookProcessor : IPostProcessor
  {
    private readonly HttpClient _client;
    private DiscordWebHookProcessorSettings? _settings;

    public DiscordWebHookProcessor()
    {
      _client = new HttpClient();
    }

    // Configure and setup the _client
    public async Task<bool> Configure(IPostProcessorSettings settings)
    {
      _settings = settings as DiscordWebHookProcessorSettings;
      if (_settings == null)
      {
        // Handle invalid settings
        return false;
      }

      // If any asynchronous configuration is needed, do it here
      // For now, we'll return a completed task
      return await Task.FromResult(true);
    }

    public async Task<PText> Process(VoiceProfile vp, PText pText)
    {
      if (_settings == null)
      {
        return null;
      }

      var name = _settings.NameFormat;
      name = name.Replace(TokenType.Name.GetToken(), _settings.Name);
      name = name.Replace(TokenType.ProfileName.GetToken(), vp.Name);
      var webhooks = _settings.ChannelURLS;

      foreach (var webhook in webhooks)
      {
        var json = JsonConvert.SerializeObject(new
        {
          content = pText.message,
          username = name,
          avatar_url = _settings.IconUrl,
        });

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
          var response = await _client.PostAsync(webhook.WebHookURL, content);

          // Optionally, check the response status
          if (!response.IsSuccessStatusCode)
          {
            //TODO handle errors
          }
        }
        catch (Exception ex)
        {
          //TODO handle errors
        }
      }

      return new PText
      {
        message = pText.message,
        voice = pText.voice
      };
    }
  }
}
