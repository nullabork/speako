using Discord;
using Discord.WebSocket;
using speako.Services.Auth;
using System.Threading.Channels;


namespace speako.Services.PostProcessors.Discord
{
  class DiscordProcessor : IPostProcessor
  {
    private DiscordProcessorSettings _settings;
    private DiscordSocketClient _client;
    private IEnumerable<IMessageChannel> messageChannels;

    //Configure and setup the _client
    public async Task Configure(IPostProcessorSettings settings)
    {
      _settings = (DiscordProcessorSettings)settings;

      _client = new DiscordSocketClient();
      await _client.LoginAsync(TokenType.Bot, _settings.BotToken);
      await _client.StartAsync();

      // Wait until the client is ready
      //_client.Ready += OnReadyAsync;

      //setup channel refernces
      messageChannels = _settings?.ChannelIds.Select(channel =>
      {
        var id = ulong.Parse(channel.ChannelId);
        return _client?.GetChannel(id) as IMessageChannel;
      });

      return;
    }

    private async Task OnReadyAsync()
    {

    }

    public async Task Process(string input)
    {
      foreach (var item in messageChannels)
      {
        await item.SendMessageAsync(input);
      }
    }
  }
}
