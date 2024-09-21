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

  public class VoiceQueue
  {
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    private readonly ConcurrentQueue<Func<Task>> _queue = new ConcurrentQueue<Func<Task>>();

    public void Enqueue(Func<Task> voiceTask)
    {
      _queue.Enqueue(voiceTask);
      ProcessQueue();
    }

    private async void ProcessQueue()
    {
      await _semaphore.WaitAsync();

      try
      {
        while (_queue.TryDequeue(out var task))
        {
          try
          {
            await task();
          }
          catch (Exception ex)
          {
            // Log the exception or handle it as needed
            Console.WriteLine($"An error occurred: {ex.Message}");
          }
        }
      }
      finally
      {
        _semaphore.Release();
      }
    }
  }



}
