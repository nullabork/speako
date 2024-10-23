using speako.Common;
using speako.Services.Auth;
using speako.Services.ProviderSettings;
using speako.Services.VoiceProfiles;
using speako.Settings;

namespace speako.Services.Providers
{
    public interface ITTSProvider
    {
        string Name { get; }

        string ToString();

        Task<bool> Configure(IAuthSettings settingsObject);

        Task<Stream> GetSpeechFromTextAsync(string text, VoiceProfiles.VoiceProfile profile, CancellationToken token);
        Task<IEnumerable<IVoice>> GetVoicesAsync(CancellationToken token);

        public VoiceProfiles.VoiceProfile DefaultVoiceProfile();

        public Task<bool> CanConnectToTTSClient();

        public object CreateClient(IAuthSettings authSettings);
  }

}
