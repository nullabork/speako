using speako.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Services.PostProcessors.DiscordWebHook
{

  public enum TokenType
  {
    Name,
    ProfileName
  }

  public static class DiscordNameTokens
  {
    public static string GetToken(this TokenType type)
    {
      switch (type)
      {
        case TokenType.Name: return "{name}";
        case TokenType.ProfileName: return "{profile name}";
        default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
      }
    }
  }
}
