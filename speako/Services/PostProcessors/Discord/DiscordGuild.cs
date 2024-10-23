using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Services.PostProcessors.Discord
{
  public class DiscordGuild
  {
    public string Name { get; set; } 
    public List<DiscordChannel> discordChannels { get; set; } = new List<DiscordChannel>();
  }
}
