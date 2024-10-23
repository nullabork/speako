using Newtonsoft.Json;
using PiperSharp;
using PiperSharp.Models;
using speako.Common;
using speako.Services.Auth;
using speako.Services.Providers.Google;

namespace speako.Services.Providers.Piper
{
  public class PiperTTSProvider: ITTSProvider
  {

    public string Name => "PiperTTS";
    private PiperTTSAuthSettings _settings;
    private PiperProvider _client;
    private static List<IVoice> _voiceCache;

    private readonly ProviderAttribute _pitchAttr = new ProviderAttribute { Default = 0, Min = -20, Max = 20 };
    private readonly ProviderAttribute _volumeAttr = new ProviderAttribute { Default = 16, Min = -96, Max = 16 };
    private readonly ProviderAttribute _speedAttr = new ProviderAttribute { Default = 1.0, Min = 0.25, Max = 4.0 };

    public override string ToString()
    {
      return Name;
    }

    public async Task<bool> Configure(IAuthSettings providerSettings)
    {
      var task = new TaskCompletionSource<bool>();
      var cwd = Directory.GetCurrentDirectory();

      if (!File.Exists(Path.Join(cwd, "piper", "piper.exe")))
      {
        await PiperDownloader.DownloadPiper().ExtractPiper(cwd);
      }

      var model = await PiperDownloader.GetModelByKey("en_GB-alan-medium");
      if (!Directory.Exists(Path.Join(cwd, "piper", "en_GB-alan-medium"))) {
        model = await model.DownloadModel(Path.Join(cwd, "piper"));
      } else {
        model = await VoiceModel.LoadModel(Path.Join(cwd, "piper", "en_GB-alan-medium"));
      }

      _client = new PiperProvider(new PiperConfiguration()
      {
        ExecutableLocation = Path.Join(cwd, "piper", "piper.exe"), // Path to piper executable
        WorkingDirectory = Path.Join(cwd, "piper"), // Path to piper working directory
        Model = model, // Loaded/downloaded VoiceModel
      });

      return await task.Task;
    }

    public object CreateClient(IAuthSettings providerSettings)
    {
      try
      {
       
      }
      catch (Exception ex)
      {

      }

      return null;
    }


    public async Task<bool> CanConnectToTTSClient()
    {
      //if _settings look at least somewhat correct
     
      return true;
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

    public async Task<Stream> GetSpeechFromTextAsync(string text, VoiceProfiles.VoiceProfile profile, CancellationToken token)
    {

      var result = await _client.InferAsync(text, AudioOutputType.Mp3);
      var ms = new MemoryStream(result);
      ms.Position = 0;

      return ms;
    }

    public async Task<IEnumerable<IVoice>> GetVoicesAsync(CancellationToken token)
    {
      if (_client == null) return new List<IVoice> { };

      if (_voiceCache != null && _voiceCache.Count() > 0)
      {
        return _voiceCache;
      }

      var models = await PiperDownloader.GetHuggingFaceModelList(); // Returns a dictionary with model key as key
      _voiceCache = models.Values.Select(v => new PiperTTSVoice(v)).ToList<IVoice>();

      return _voiceCache;
    }
  }
}
