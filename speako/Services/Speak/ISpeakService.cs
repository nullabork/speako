
using speako.Services.Providers;
using speako.Services.VoiceSettings;

namespace speako.Services.Speak
{
  public interface ISpeakService
  {
    Task SpeakText(ITTSProvider ttsProvider, VoiceProfile profile, string text);
  }
}