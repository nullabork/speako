using speako.Common;
using speako.Services.Auth;
using speako.Services.ProviderSettings;
using speako.Services.VoiceSettings;
using speako.Settings;

namespace speako.Services.Providers
{
    public interface ITTSProvider
    {
        string Name { get; }

        string ToString();

        public IProviderSettingsControl SettingsControl();

        void LoadSettings(IAuthSettings settingsObject);

        Task<Stream> GetSpeechFromTextAsync(string text, VoiceProfile profile, CancellationToken token);
        Task<IEnumerable<IVoice>> GetVoicesAsync(CancellationToken token);

        public VoiceProfile DefaultVoiceProfile();

        public Task<bool> CanConnectToTTSClient();

        public object CreateClient(IAuthSettings authSettings);
  }

}
