using NAudio.Wave;

namespace speako.Services.Audio
{
  public interface IAudioService
  {
    Device GetDirectSoundOut(Guid deviceId);
  }
}


