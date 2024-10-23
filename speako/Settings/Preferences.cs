using System.ComponentModel;

namespace speako.Settings
{
  public class Preferences: JsonSerializable<Preferences>, INotifyPropertyChanged
  {
    public bool AlwaysOnTop { get; set; }

    public string Theme { get; set; }

    public string Cheese { get; set; }
    public string CastType => this.GetType().ToString();

    public event PropertyChangedEventHandler? PropertyChanged;
  }
}
