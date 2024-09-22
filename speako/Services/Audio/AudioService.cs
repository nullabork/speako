using NAudio.Wave;

using System.Collections.Concurrent;

namespace speako.Services.Audio
{
  public class AudioService : IAudioService
  {
    private readonly ConcurrentDictionary<Guid, DeviceHandler> _devices = new ConcurrentDictionary<Guid, DeviceHandler>();

    public DeviceHandler GetDirectSoundOut(Guid deviceId)
      => _devices.GetOrAdd(deviceId, (id) => BuildDirectSoundOut(id));

    private DeviceHandler BuildDirectSoundOut(Guid id)
    {
      var device = new DeviceHandler();
      device.DirectSound = new DirectSoundOut(id);
      device.DirectSound.PlaybackStopped += (sender, e) => device.CompletionSource.SetResult(true);
      return device;
    }
  }

  public class DeviceHandler
  {
    public DirectSoundOut? DirectSound { get; set; }
    public TaskCompletionSource<bool> CompletionSource { get; set; } = new TaskCompletionSource<bool>();
  }

}


