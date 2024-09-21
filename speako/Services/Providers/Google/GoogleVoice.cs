using Google.Cloud.TextToSpeech.V1;
using speako.Services.Auth;
using speako.Settings;

namespace speako.Services.Providers.Google
{
  internal class GoogleVoice : IVoice
  {
    private Voice v;

    private IAuthSettings cp;

    public GoogleVoice(Voice v)
    {
      this.v = v;
      //this.cp = cp;
    }

    public string Name => v.Name;

    public string Language => v.LanguageCodes[0];

    public override string ToString()
    {
      return v.Name;
    }
  }
}