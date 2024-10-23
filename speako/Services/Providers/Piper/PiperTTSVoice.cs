
using PiperSharp.Models;
using speako.Services.Auth;
using speako.Settings;

namespace speako.Services.Providers.Piper
{
  internal class PiperTTSVoice : IVoice
  {
    private VoiceModel v;

    public PiperTTSVoice(VoiceModel v)
    {
      this.v = v;
    }

    public string Name => v.Name;

    public string Language => v.Language.Code;

    public override string ToString()
    {
      return v.Name;
    }
  }
}