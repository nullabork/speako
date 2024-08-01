namespace speako.Services.Providers
{
    public interface ITTSProvider
    {
        Task<Stream> GetSpeechFromTextAsync(string text, CancellationToken token);
        Task<IEnumerable<IVoice>> GetVoicesAsync(CancellationToken token);
    }

}
