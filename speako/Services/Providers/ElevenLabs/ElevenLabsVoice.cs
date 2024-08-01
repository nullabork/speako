namespace speako.Services.Providers.ElevenLabs
{
  internal class ElevenLabsVoice : IVoice
  {
    public string Id { get; }
    public string Name { get; }

    public string Language => "asdasd";

    public ElevenLabsVoice(string id, string name)
    {
      Id = id;
      Name = name;
    }
  }
}