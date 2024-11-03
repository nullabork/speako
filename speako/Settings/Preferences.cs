using System.ComponentModel;

namespace speako.Settings
{
  public class Preferences: JsonSerializable<Preferences>, INotifyPropertyChanged, IDisposable
  {
    public bool AlwaysOnTop { get; set; }

    public string Theme { get; set; }

    public string DataLocation { get; set; }
    public string Cheese { get; set; }
    public string CastType => this.GetType().ToString();

    public event PropertyChangedEventHandler? PropertyChanged;

    public Preferences()
    {
      Saved += Preferences_Saved;
    }

    private void Preferences_Saved(object? sender, Preferences e)
    {
      var settings = AppConfig.Default.SaveLocation;
    }

    public void Dispose()
    {
      Saved -= Preferences_Saved;
    }
  }
}
