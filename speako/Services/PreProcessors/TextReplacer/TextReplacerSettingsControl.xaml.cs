using speako.Common;
using speako.Services.Auth;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace speako.Services.PreProcessors.TextReplacer
{
  /// <summary>
  /// Interaction logic for TextReplacerSettingsControl.xaml
  /// </summary>
  public partial class TextReplacerSettingsControl : UserControl, IPreProcessorControl
  {

    private IPreProcessorSettings _original;
    private IPreProcessorSettings _working;
    public TextReplacerSettingsControl()
    {
      InitializeComponent();
    }

    public event EventHandler<IPreProcessorSettings> Saved;
    public event EventHandler<IPreProcessorSettings> Cancel;

    public async Task Configure(IPreProcessorSettings settings)
    {
      var og = (TextReplacerProcessorSettings)settings;
      var current = (TextReplacerProcessorSettings)ObjectUtils.Clone(settings);

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
      var isEqual = Compare.ObjectsPropertiesEqual((TextReplacerProcessorSettings)_working, (TextReplacerProcessorSettings)_original);
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

    private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      SaveButtonState();
    }
  }
}
