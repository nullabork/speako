using Accord.IO;
using Omu.ValueInjecter;
using speako.Common;
using speako.Services.Auth;
using speako.Services.Providers;
using speako.Services.ProviderSettings;
using speako.Settings;
using System.Windows;
using System.Windows.Input;

namespace speako.Services.PostProcessors
{
  /// <summary>
  /// Interaction logic for PostProcessorListingWindow.xaml
  /// </summary>
  public partial class PostProcessorListingWindow : Window
  {
    private IEnumerable<IPostProcessorSettings> _processors;
    private readonly ApplicationSettings _applicationSettings;
    public PostProcessorListingWindow(IEnumerable<IPostProcessorSettings> processorSettings, ApplicationSettings appSettings)
    {
      InitializeComponent();
      _processors = processorSettings;
      _applicationSettings = appSettings;

      processorsComboBox.ItemsSource = _processors;
      DataContext = _applicationSettings;
    }

    public void EditProcessor(IPostProcessorSettings settings)
    {
      var control = settings.GetSettingsControl();
      control.Configure(settings);

      PostProcessorDetailWindow settingsWindow = new PostProcessorDetailWindow();
      settingsWindow.settingsScroller.Content = control;

      control.Saved += (sender, e) =>
      {
        var find = _applicationSettings.PostProcessors.FirstOrDefault((IPostProcessorSettings settings) => settings.GUID == e.GUID, null);
        var index = _applicationSettings.PostProcessors.IndexOf(find);

        if (find != null && index != -1)
        {
          find.InjectFrom(e);
        }
        else
        {
          _applicationSettings.PostProcessors.Add(e);
         
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
      var settings = (IPostProcessorSettings)Activator.CreateInstance(ps.GetType());
      EditProcessor(settings);
    }

    private void Test_Click(object sender, RoutedEventArgs e)
    {
      
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
      var settings = (IPostProcessorSettings)ProcessorsListBox.SelectedItem;
      EditProcessor(settings);
    }
    private void DuplicateSettings(IPostProcessorSettings settings)
    {
      var settingsCopy = settings.Duplicate();
      settingsCopy.Name = $"{settingsCopy.Name} - copy";
      settingsCopy.Init();
      var sIndex = _applicationSettings.PostProcessors.IndexOf(settings);
      _applicationSettings.PostProcessors.Insert(sIndex + 1, settingsCopy);
      _applicationSettings.Save();
    }

    private void Duplicate_Click(object sender, RoutedEventArgs e)
    {
      var item = (IPostProcessorSettings)ProcessorsListBox.SelectedItem;
      if (item != null)
      {
        DuplicateSettings(item);
      }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
      var item = (IPostProcessorSettings)ProcessorsListBox.SelectedItem;
      if (item != null)
      {
        MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {item.Name ?? "this item"}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
          _applicationSettings.PostProcessors.Remove(item);
          _applicationSettings.Save();
        }
      }

    }

    private void ProcessorsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var settings = (IPostProcessorSettings)ProcessorsListBox.SelectedItem;
      EditProcessor(settings);
    }
  }
}
