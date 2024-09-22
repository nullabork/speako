using NAudio.Wave;

namespace speako.Services.Audio
{
  public interface IAudioService
  {
    DeviceHandler GetDirectSoundOut(Guid deviceId);
  }
}


