using NAudio.Wave;

using speako.Services.Providers;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Common
{
  using System.Collections.Concurrent;

  public class VoiceQueue : IAsyncDisposable
  {
    private readonly SemaphoreSlim _enqueueSempaphore = new SemaphoreSlim(1, 1);
    private readonly SemaphoreSlim _processingSemaphore = new SemaphoreSlim(0, 1);
    private readonly Queue<Func<CancellationToken, Task>> _queue = new Queue<Func<CancellationToken, Task>>();
    private readonly Task _processing;
    private readonly CancellationTokenSource _cancel = new CancellationTokenSource();

    public VoiceQueue()
    {
      _processing = ProcessQueue();
    }

    public async ValueTask DisposeAsync()
    {
      _cancel.Cancel();
      await _processing;
      _enqueueSempaphore.Dispose();
      _processingSemaphore.Dispose();
    }

    public async Task Enqueue(Func<CancellationToken, Task> voiceTask)
    {
      try
      {
        await _enqueueSempaphore.WaitAsync();
        System.Diagnostics.Debug.WriteLine("Enqueue");
        _queue.Enqueue(voiceTask);
        _processingSemaphore.TryRelease();
      }
      catch(Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.ToString());
      }
      finally
      {
        _enqueueSempaphore.Release();
        System.Diagnostics.Debug.WriteLine("Enqueue - Release");
      }
    }

    private async Task ProcessQueue()
    {
      try
      {
        while (!_cancel.Token.IsCancellationRequested)
        {
          System.Diagnostics.Debug.WriteLine("Process");
          if (_queue.Count == 0)
          {
            System.Diagnostics.Debug.WriteLine("Process - Wait");
            await _processingSemaphore.WaitAsync(_cancel.Token);
          }
          if (_cancel.Token.IsCancellationRequested) break;
          if (!_queue.TryDequeue(out var task))
            continue;

          try
          {
            await task(_cancel.Token);
            System.Diagnostics.Debug.WriteLine("Processed");
          }
          catch (Exception ex)
          {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
          }
        }
      }
      catch( Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.ToString());
      }
    }
  }

  public static class SemaphoreSlimEx
  {
    public static bool TryRelease(this SemaphoreSlim semaphoreSlim)
    {
      try { semaphoreSlim.Release(); } catch (SemaphoreFullException) { return false; }
      return true;
    }
  }
}
