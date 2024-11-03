
using PiperSharp.Models;
using speako.Common;
using speako.Services.Auth;
using speako.Settings;
using System.ComponentModel;
using System.Text.Json;

namespace speako.Services.Providers.Piper
{

  public class PiperModelFileInfo
  {
    public long size_bytes { get; set; }  // Match the JSON property name
    public string md5_digest { get; set; }  // Match the JSON property name
  }

  public class PiperTTSVoice : IVoice, INotifyPropertyChanged
  {
    public PiperTTSVoice(VoiceModel v)
    {
     
      this.Name = v.Name;
      this.Id = v.Key;
      this.Language = v.Language.Code;

      var total = v.Files.Values.Select((v) => {

        PiperModelFileInfo fileInfo = JsonSerializer.Deserialize<PiperModelFileInfo>(v);
        return fileInfo.size_bytes;

      }).Sum();

      this.Size = Prettier.FormatSizeUnits(total);
    }

    public string Name { get; set; }// => v.Name;

    public string Language { get; set; }// => v.Language.Code;

    public string Id { get; set; }// => v.Key;

    public string Size {  get; set; }

    public bool Downloaded { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
  }
}