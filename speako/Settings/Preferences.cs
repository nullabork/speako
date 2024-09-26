using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace speako.Settings
{
  public class Preferences: JsonSerializable<Preferences>, INotifyPropertyChanged
  {
    public Preferences() { }

    public string AudioDeviceGUID { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
  }
}
