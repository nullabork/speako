using Discord;
using Discord.WebSocket;
using speako.Services.Speak;
using speako.Services.VoiceProfiles;
using System.Threading.Channels;


namespace speako.Services.PostProcessors.Discord
{
  class DiscordProcessor : IPostProcessor
  {
    private DiscordProcessorSettings _settings;
    private DiscordSocketClient _client;
    private IEnumerable<IMessageChannel> _messageChannels;
    private List<DiscordGuild> _cachedChannels = new List<DiscordGuild>();


    //Configure and setup the _client
    public async Task<bool> Configure(IPostProcessorSettings settings)
    {
      //completeTask
      var task = new TaskCompletionSource<bool>();
      var discordSettings = (DiscordProcessorSettings)settings;

      if (discordSettings?.BotToken == _settings?.BotToken)
      {
        task.SetResult(false);
        return await task.Task;
      }

      _cachedChannels.Clear();
      _settings = discordSettings;


      var config = new DiscordSocketConfig
      {
        // Enable Guilds intent to access guild and channel info
        GatewayIntents = GatewayIntents.Guilds
      };

      _client = new DiscordSocketClient(config);
      await _client.LoginAsync(TokenType.Bot, _settings?.BotToken);
      await _client.StartAsync();

    

      // Wait until the client is ready
      _client.Ready += async () =>
      {
        task.SetResult(true);
      };


      //setup channel refernces
      _messageChannels = _settings?.ChannelIds.Select(channel =>
      {
        var id = ulong.Parse(channel.ChannelId);
        return _client?.GetChannel(id) as IMessageChannel;
      });

      return await task.Task;
    }

    public async Task<List<DiscordGuild>> GetDiscordChannels()
    {
      if (_client == null)
      {
        return [];
      }

      if (_cachedChannels.Count > 0)
      {
        return _cachedChannels;
      }

      var guilds = new List<DiscordGuild>();

      foreach (var guild in _client.Guilds)
      {
        var channels = guild.Channels;
        var g = new DiscordGuild
        {
          Name = guild.Name,
        };

        foreach (var channel in channels)
        {
          g.discordChannels.Add(new DiscordChannel
          {
            Name = $"{guild.Name} - {channel.Name}",
            ChannelId = channel.Id.ToString(),
            ChannelName = channel.Name,
            GuildName = guild.Name,
          });
        }

        guilds.Add(g);
      }

      return guilds;
    }

    public async Task<PText> Process(VoiceProfile vp, PText pText)
    {
      foreach (var item in _messageChannels)
      {
        await item.SendMessageAsync(pText.message);
      }

      return new PText
      {
        message = pText.message,
        voice = pText.voice,
      };
    }
  }
}
