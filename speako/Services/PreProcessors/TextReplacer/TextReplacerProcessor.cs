

using Accord.Math;
using speako.Services.Speak;
using speako.Services.VoiceProfiles;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace speako.Services.PreProcessors.TextReplacer
{
  class TextReplacerProcessor : IPreProcessor, IDisposable
  {


    private TextReplacerProcessorSettings _settings;
    private Dictionary<string, string> _voiceReplacements { get; set; } = new Dictionary<string, string>();
    private Dictionary<string, string> _messageReplacements { get; set; } = new Dictionary<string, string>();
    private string _voiceReplacePattern { get; set; }
    private string _messageReplacePattern { get; set; }

    public async Task<PText>? Process(VoiceProfile vp, PText text)
    {
      var voice = Regex.Replace(text.voice, _voiceReplacePattern, m => _voiceReplacements[m.Value]);
      var message = Regex.Replace(text.message, _messageReplacePattern, m => _messageReplacements[m.Value]);

      return new PText
      {
        message = message,
        voice = voice
      };
    }

    public async Task<bool> Configure(IPreProcessorSettings settings)
    {
      //completeTask
      var task = new TaskCompletionSource<bool>();
      _settings = (TextReplacerProcessorSettings)settings;
      _settings.PropertyChanged += Settings_PropertyChanged;

      CompileReplacements();

      return await task.Task;
    }

    private void CompileReplacements()
    {
      _voiceReplacements.Clear();
      _messageReplacements.Clear();

      foreach (var replacement in _settings.Replacements)
      {
        if (replacement == null) continue;
        if (string.IsNullOrEmpty(replacement.From)) continue;
        if (!string.IsNullOrEmpty(replacement.MessageText))
        {
          _messageReplacements.TryAdd(replacement.From, replacement.MessageText);
        }

        if (!string.IsNullOrEmpty(replacement.VoiceText))
        {
          _voiceReplacements.TryAdd(replacement.From, replacement.VoiceText);
        }
      }


      _voiceReplacePattern = string.Join("|",
        _voiceReplacements.Keys
          .Select(k => Regex.Escape(k))
          .OrderByDescending(k => k.Length)
      );

      _messageReplacePattern = string.Join("|",
        _messageReplacements.Keys
          .Select(k => Regex.Escape(k))
          .OrderByDescending(k => k.Length)
      );
    }


    private void Settings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      CompileReplacements();
    }

    public void Dispose()
    {
      
    }
  }
}
