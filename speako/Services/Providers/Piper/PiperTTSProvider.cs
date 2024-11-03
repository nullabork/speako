using PiperSharp;
using PiperSharp.Models;
using speako.Common;
using speako.Services.Auth;
using speako.Services.VoiceProfiles;

namespace speako.Services.Providers.Piper
{
  public class PiperTTSProvider: ITTSProvider
  {

    public string Name => "PiperTTS";
    private PiperTTSAuthSettings _settings;
    private PiperProvider _client;
    private static List<IVoice> _voiceCache;
    private static List<PiperTTSVoice> _HFVoiceCache;
    private static Dictionary<string, VoiceModel> _modelCache;

    private readonly ProviderAttribute _pitchAttr = new ProviderAttribute { Default = 0, Min = -20, Max = 20 };
    private readonly ProviderAttribute _volumeAttr = new ProviderAttribute { Default = 16, Min = -96, Max = 16 };
    private readonly ProviderAttribute _speedAttr = new ProviderAttribute { Default = 1.0, Min = 0.25, Max = 4.0 };

    public override string ToString()
    {
      return Name;
    }

    public static bool ModelIsDownloaded(string id)
    {
      var path = JsonConfigTools.GetDataDirectory();
      return Directory.Exists(Path.Join(path, "piper", id));
    }

    public static async Task<VoiceModel> GetModel(string id)
    {

      if (_modelCache == null)
      {
        _modelCache = new Dictionary<string, VoiceModel>();
      }

      if (_modelCache.ContainsKey(id))
      {
        return _modelCache.GetValueOrDefault(id, null);
      }

      var path = JsonConfigTools.GetDataDirectory();
      var model = await PiperDownloader.GetModelByKey(id);
      if (!ModelIsDownloaded(id))
      {
        model = await model.DownloadModel(Path.Join(path, "piper"));
      }
      else
      {
        model = await VoiceModel.LoadModel(Path.Join(path, "piper", id));
      }

      _modelCache.Add(id, model);
      return model;
    }

    public async Task<bool> Configure(IAuthSettings providerSettings)
    {
      var task = new TaskCompletionSource<bool>();
      _client = (PiperProvider)await CreateClient();

      task.SetResult(true);

      return await task.Task;
    }

    public async Task<object> CreateClient()
    {

      if (_client != null)
      {
        return _client;
      }

      try
      {
        var piperpath = Directory.GetCurrentDirectory();
        var path = JsonConfigTools.GetDataDirectory();

        if (!File.Exists(Path.Join(piperpath, "piper", "piper.exe")))
        {
          await PiperDownloader.DownloadPiper().ExtractPiper(path);
        }
        
        _client = new PiperProvider(new PiperConfiguration()
        {
          ExecutableLocation = Path.Join(piperpath, "piper", "piper.exe"), // Path to piper executable
          WorkingDirectory = Path.Join(piperpath, "piper"), // Path to piper working directory
          //Model = model
        });
      }
      catch (Exception ex)
      {

      }


      //var cwd = Directory.GetCurrentDirectory();

      //if (!File.Exists(Path.Join(cwd, "piper", "piper.exe")))
      //{
      //  await PiperDownloader.DownloadPiper().ExtractPiper(cwd);
      //}

      //var model = await PiperDownloader.GetModelByKey("en_GB-cori-medium");
      //if (!Directory.Exists(Path.Join(cwd, "piper", "en_GB-cori-medium")))
      //{
      //  model = await model.DownloadModel(Path.Join(cwd, "piper"));
      //}
      //else
      //{
      //  model = await VoiceModel.LoadModel(Path.Join(cwd, "piper", "en_GB-cori-medium"));
      //}

      //_client = new PiperProvider(new PiperConfiguration()
      //{
      //  ExecutableLocation = Path.Join(cwd, "piper", "piper.exe"), // Path to piper executable
      //  WorkingDirectory = Path.Join(cwd, "piper"), // Path to piper working directory
      //  Model = model, // Loaded/downloaded VoiceModel
      //});

      return _client;
    }


    public async Task<bool> CanConnectToTTSClient()
    {
      //if _settings look at least somewhat correct
     
      return true;
    }

    public static string GetPiperDataDirectory()
    {
      return Path.Combine(JsonConfigTools.GetDataDirectory(), "piper");
    }

    public VoiceProfiles.VoiceProfile DefaultVoiceProfile()
    {
      return new VoiceProfiles.VoiceProfile
      {
        Pitch = _pitchAttr.GetDefault(),
        Volume = _volumeAttr.GetDefault(),
        Speed = _speedAttr.GetDefault(),
      };
    }

    public async Task<Stream> GetSpeechFromTextAsync(string text, VoiceProfile profile, CancellationToken token)
    {
      var model = await GetModel(profile.VoiceID);
      _client.Configuration.Model = model;

      var result = await _client.InferAsync(text, AudioOutputType.Mp3);
      var ms = new MemoryStream(result);
      ms.Position = 0;

      return ms;
    }

    public static async Task<IEnumerable<PiperTTSVoice>> GetAvailableVoicesAsync()
    {
      if (_HFVoiceCache != null && _HFVoiceCache.Count() > 0)
      {
        return _HFVoiceCache;
      }
      
      var models = await PiperDownloader.GetHuggingFaceModelList(); // Returns a dictionary with model key as key
      _HFVoiceCache = models.Values.Select(v => new PiperTTSVoice(v)).ToList();

      _HFVoiceCache.ForEach(v =>
      {
        var path = Path.Combine(GetPiperDataDirectory(), v.Id);
        v.Downloaded = Directory.Exists(path);
      });

      return _HFVoiceCache;
    }

    public async Task<IEnumerable<IVoice>> GetVoicesAsync(CancellationToken token)
    {
      return _HFVoiceCache.FindAll(v => v.Downloaded);
    }
  }
}
