using NAudio.Wave;

using System.Collections.Concurrent;

namespace speako.Services.Audio
{
  public class AudioService : IAudioService
  {
    private readonly ConcurrentDictionary<Guid, Device> _devices = new ConcurrentDictionary<Guid, Device>();

    public Device GetDirectSoundOut(Guid deviceId)
      => _devices.GetOrAdd(deviceId, (id) => BuildDirectSoundOut(id));

    private Device BuildDirectSoundOut(Guid id)
    {
      var device = new Device();
      device.DirectSound = new DirectSoundOut(id);
      device.DirectSound.PlaybackStopped += (sender, e) => device.CompletionSource.SetResult(true);
      return device;
    }
  }

  public class Device
  {
    public DirectSoundOut? DirectSound { get; set; }
    public TaskCompletionSource<bool> CompletionSource { get; set; } = new TaskCompletionSource<bool>();
  }

}


