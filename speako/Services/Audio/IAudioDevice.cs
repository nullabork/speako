namespace speako.Services.Audio
{
  public interface IAudioDevice
  {
    public string DeviceName { get; set; }
    public string DeviceGuid { get; set; }
    public string ToString();
  }
}

