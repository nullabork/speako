using speako.Common;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace speako.Services.PostProcessors.DiscordWebHook
{
  /// <summary>
  /// Interaction logic for DiscordWebHookSettingsControl.xaml
  /// </summary>
  public partial class DiscordWebHookSettingsControl : UserControl, IPostProcessorControl
  {

    private IPostProcessorSettings _original;
    private IPostProcessorSettings _working;
    public DiscordWebHookSettingsControl()
    {
      InitializeComponent();
    }

    public event EventHandler<IPostProcessorSettings> Saved;
    public event EventHandler<IPostProcessorSettings> Cancel;

    public async Task Configure(IPostProcessorSettings settings)
    {

      var og = (DiscordWebHookProcessorSettings)settings;
      var current = (DiscordWebHookProcessorSettings)ObjectUtils.Clone(settings);
      current.ChannelURLS = new ObservableCollection<DiscordChannelWebHook>(og.ChannelURLS);

      _original = og;
      _working = current;

      _working.PropertyChanged += OnPropertyChanged;
      this.DataContext = _working;
      SaveButtonState();
      return;
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      SaveButtonState();
    }

    private void SaveButtonState()
    {
      var isEqual = Compare.ObjectsPropertiesEqual((DiscordWebHookProcessorSettings)_working, (DiscordWebHookProcessorSettings)_original);
      saveButton.IsEnabled = !isEqual && !string.IsNullOrEmpty(_working.Name);
    }

    private void saveButton_Click(object sender, RoutedEventArgs e)
    {
      
      Saved.Invoke(this, _working);
      Configure(_working);
    }

    private void cancelButton_Click(object sender, RoutedEventArgs e)
    {
      Cancel.Invoke(this, _working);
    }

  }
}
