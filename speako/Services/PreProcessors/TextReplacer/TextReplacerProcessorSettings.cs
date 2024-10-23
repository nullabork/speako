using Accord.IO;
using FastDeepCloner;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace speako.Services.PreProcessors.TextReplacer
{
  class TextReplacerProcessorSettings : IPreProcessorSettings, INotifyPropertyChanged
  {
    public string DisplayName => "TextReplacer Settings";

    public IPreProcessorControl GetSettingsControl()
    {
      return new TextReplacerSettingsControl();
    }

    private IPreProcessor _processor;
    public IPreProcessor GetPreProcessor()
    {
      if (_processor == null) {
        _processor = new TextReplacerProcessor();
      }

      return _processor;
    }

    public async void Init()
    {
      _processor = null;
      _processor = GetPreProcessor();
      _processor.Configure(this);
    }

    //just unique id used for references
    public string GUID { get; set; } = Guid.NewGuid().ToString();

    //Name purely used for display in the UI
    public string Name { get; set; }

    //Actual config for TextReplacer.net
    public ObservableCollection<MessageReplacement> Replacements { get; set; } = new ObservableCollection<MessageReplacement>();


    //Cast type is used by the newtonSoft json so it knows what class to create when deserialising
    public string CastType => this.GetType().ToString();


    //helps with the window controls
    public event PropertyChangedEventHandler PropertyChanged;

    public IPreProcessorSettings Duplicate()
    {
      var cloned = DeepCloner.Clone(this);
      cloned.GUID = Guid.NewGuid().ToString();
      return cloned;
    }
  }
}
