
using speako.Services.VoiceProfiles;
using System.Collections.ObjectModel;

namespace speako.Services.Speak
{

  public class SessionService
  {

    public ObservableCollection<MessageSession> MessageSessions { get; set; } = new ObservableCollection<MessageSession>();

    public List<Func<Task<MessageSession>>> loadQueue = new List<Func<Task<MessageSession>>>();

    public string FileNamePrefix => "message_session_";
    private DateTime _lastLoadTime = DateTime.MinValue; // To track when the last item was loaded
    private const int LoadCooldownMilliseconds = 1000;


    public SessionService()
    {

      var ms = new MessageSession();
      var configBase = ms.GetFileBase();

      if (Directory.Exists(configBase))
      {
        var files = Directory.GetFiles(configBase, $"{FileNamePrefix}*")
                                     .OrderBy(f => File.GetCreationTime(f)) // You can also use GetLastWriteTime if needed
                                     .ToArray();

        foreach (var file in files)
        {
          loadQueue.Add(async () =>
          {
            var filename = Path.GetFileName(file);
            var messageSession = new MessageSession
            {
              FileName = filename.Replace(".json", ""),
            };
            await messageSession.Load(); // Assuming Load is an async method
            return messageSession;
          });
        }
      }
      else
      {
        Console.WriteLine($"Directory '{configBase}' does not exist.");
      }
    }

    public async Task LoadNext()
    {
      await LoadNext(1);
    }

    public async Task LoadNext(int loadSessions)
    {
      // If loadQueue is empty, don't do anything
      if (loadQueue.Count == 0)
      {
        return;
      }

      // Enforce the cooldown period
      if ((DateTime.Now - _lastLoadTime).TotalMilliseconds < LoadCooldownMilliseconds)
      {
        return;
      }

      // Determine the number of sessions to load
      int sessionsToLoad = Math.Min(loadSessions, loadQueue.Count);

      for (int i = 0; i < sessionsToLoad; i++)
      {
        // Remove the last function from the queue and execute it
        var loadFunc = loadQueue[^1]; // ^1 accesses the last element
        loadQueue.RemoveAt(loadQueue.Count - 1);

        var messageSession = await loadFunc();
        MessageSessions.Insert(0, messageSession);
      }

      _lastLoadTime = DateTime.Now; // Update the last load time
    }

    public async Task AddMessage(string message, VoiceProfiles.VoiceProfile? voiceProfile)
    {
      var latestSession = MessageSessions.LastOrDefault();
      if (latestSession == null || latestSession.DateTime.Date != DateTime.Today)
      {
        latestSession = new MessageSession
        {
          FileName = $"{FileNamePrefix}{DateTime.Now.ToString("yyyyMMddHHmmss")}",
          DateTime = DateTime.Now
        };

        MessageSessions.Add(latestSession);
      }

      latestSession.Messages.Add(new TextMessage
      {
        Message = message,
        VoiceProfileName = voiceProfile?.Name ?? ""
      });

      latestSession.Save();
    }

    public async Task AddMessage(string message)
    {
      await AddMessage(message, null);
    }
  }
}
