using Accord.IO;
using Omu.ValueInjecter;
using speako.Services.Auth;
using speako.Services.PostProcessors;
using speako.Services.Providers;
using speako.Services.ProviderSettings;
using speako.Settings;
using System.Windows;
using System.Windows.Input;

namespace speako.Services.PreProcessors
{
  /// <summary>
  /// Interaction logic for PreProcessorListingWindow.xaml
  /// </summary>
  public partial class PreProcessorListingWindow : Window
  {
    private IEnumerable<IPreProcessorSettings> _processors;
    private readonly ApplicationSettings _applicationSettings;
    public PreProcessorListingWindow(IEnumerable<IPreProcessorSettings> processorSettings, ApplicationSettings appSettings)
    {
      InitializeComponent();
      _processors = processorSettings;
      _applicationSettings = appSettings;

      processorsComboBox.ItemsSource = _processors;
      DataContext = _applicationSettings;
    }

    public void EditProcessor(IPreProcessorSettings settings)
    {
      var control = settings.GetSettingsControl();
      control.Configure(settings);

      PreProcessorDetailWindow settingsWindow = new PreProcessorDetailWindow();
      settingsWindow.settingsScroller.Content = control;

      control.Saved += (sender, e) =>
      {
        var find = _applicationSettings.PreProcessors.FirstOrDefault((IPreProcessorSettings settings) => settings.GUID == e.GUID, null);
        var index = _applicationSettings.PreProcessors.IndexOf(find);

        if (find != null && index != -1)
        {
          find.InjectFrom(e);
        }
        else
        {
          _applicationSettings.PreProcessors.Add(e);
          _applicationSettings.ProcessProviders();
        }

        _applicationSettings.Save();
      };

      control.Cancel += (sender, e) =>
      {
        settingsWindow.Close();
      };

      settingsWindow.ShowDialog();
    }

    private void addProcessorButton_Click(object sender, RoutedEventArgs e)
    {
      var ps = processorsComboBox.SelectedItem;
      var settings = (IPreProcessorSettings)Activator.CreateInstance(ps.GetType());
      EditProcessor(settings);
    }

    private void Test_Click(object sender, RoutedEventArgs e)
    {
      
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
      var settings = (IPreProcessorSettings)ProcessorsListBox.SelectedItem;
      EditProcessor(settings);
    }
    private void DuplicateSettings(IPreProcessorSettings settings)
    {
      var settingsCopy = settings.Duplicate();
      settingsCopy.Name = $"{settingsCopy.Name} - copy";
      settingsCopy.Init();
      var sIndex = _applicationSettings.PreProcessors.IndexOf(settings);
      _applicationSettings.PreProcessors.Insert(sIndex + 1, settingsCopy);
      _applicationSettings.Save();
    }

    private void Duplicate_Click(object sender, RoutedEventArgs e)
    {
      var item = (IPreProcessorSettings)ProcessorsListBox.SelectedItem;
      if (item != null)
      {
        DuplicateSettings(item);
      }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
      var item = (IPreProcessorSettings)ProcessorsListBox.SelectedItem;
      if (item != null)
      {
        MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {item.Name ?? "this item"}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
          _applicationSettings.PreProcessors.Remove(item);
          _applicationSettings.Save();
        }
      }

    }

    private void ProcessorsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var settings = (IPreProcessorSettings)ProcessorsListBox.SelectedItem;
      EditProcessor(settings);
    }
  }
}
