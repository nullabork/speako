using Google.Cloud.TextToSpeech.V1;
using speako.Settings;

namespace speako.Services.Providers.Google
{
  internal class GoogleVoice : IVoice
  {
    private Voice v;

    private ConfiguredProvider cp;

    public GoogleVoice(Voice v, ConfiguredProvider cp)
    {
      this.v = v;
      this.cp = cp;
    }

    public string Name => v.Name;

    public string Language => v.LanguageCodes[0];

    public string ConfuredProviderUUID => cp.uuid;

    public override string ToString()
    {
      return v.Name;
    }
  }
}