namespace speako.Features.Speak.Providers
{
    public interface IVoice
    {
        string Name { get; }
        string Language { get; }
    }
}