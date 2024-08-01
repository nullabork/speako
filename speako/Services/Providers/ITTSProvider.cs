namespace speako.Services.Providers
{
    public interface ITTSProvider
    {
        string Name { get; }
        Task<Stream> GetSpeechFromTextAsync(string text, CancellationToken token);
        Task<IEnumerable<IVoice>> GetVoicesAsync(CancellationToken token);
    }

}
