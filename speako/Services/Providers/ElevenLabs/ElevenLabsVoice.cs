namespace speako.Services.Providers.ElevenLabs
{
  internal class ElevenLabsVoice : IVoice
  {
    public string Id { get; }
    public string Name { get; }

    public string Language => "asdasd";

    public string ConfuredProviderUUID => throw new NotImplementedException();

    public ElevenLabsVoice(string id, string name)
    {
      Id = id;
      Name = name;
    }
  }
}