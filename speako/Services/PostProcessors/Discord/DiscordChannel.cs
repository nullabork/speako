using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Services.PostProcessors.Discord
{
  public class DiscordChannel
  {

    public string GuildName { get; set; }

    public string Name { get; set; }

    public string ChannelName { get; set; }

    public string ChannelId { get; set; }

    public string CastType => this.GetType().ToString();
  }
}
