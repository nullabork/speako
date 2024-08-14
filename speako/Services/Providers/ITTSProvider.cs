using speako.Settings;

namespace speako.Services.Providers
{
    public interface ITTSProvider
    {
        string Name { get; }

        string ToString();

        void OpenSettings();

        void LoadSettings(ConfiguredProvider cp);

        Task<Stream> GetSpeechFromTextAsync(string text, CancellationToken token);
        Task<IEnumerable<IVoice>> GetVoicesAsync(CancellationToken token);
    }

}
