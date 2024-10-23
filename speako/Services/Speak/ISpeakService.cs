using speako.Services.VoiceProfiles;

namespace speako.Services.Speak
{
  public interface ISpeakService
  {
    Task SpeakText(string message, VoiceProfile profile);
  }
}