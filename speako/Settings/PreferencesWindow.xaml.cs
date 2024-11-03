using Omu.ValueInjecter;
using speako.Common;
using speako.Services.Audio;
using System.ComponentModel;
using System.Windows;
using speako.Themes;
using System.Windows.Forms;
using System.Configuration;

namespace speako.Settings
{
  /// <summary>
  /// Interaction logic for PreferencesWindow.xaml
  /// </summary>
  public partial class PreferencesWindow : Window
  {
    private readonly AudioDevicesService _AudioDevicesService;

    private Preferences _workingPreferences;
    private Preferences _originalPreferences;


    public PreferencesWindow(AudioDevicesService devices, Preferences preferences)
    {
      InitializeComponent();
      _originalPreferences = preferences;
      _workingPreferences = ObjectUtils.Clone(preferences);

      _AudioDevicesService = devices;


      DataContext = _workingPreferences;
      SaveButtonState();
      _workingPreferences.PropertyChanged += OnPropertyChanged;

      var themeList = Enum.GetValues(typeof(ThemeType))
                    .Cast<ThemeType>()
                    .Select(t => t.GetName())
                    .ToList();

      themesComboBox.ItemsSource = themeList;

    }

    private void SaveButtonState()
    {
      var isEqual = Compare.ObjectsPropertiesEqual(_originalPreferences, _workingPreferences);
      saveButton.IsEnabled = !isEqual;
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      SaveButtonState();
    }

    private void cancelButton_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void saveButton_Click(object sender, RoutedEventArgs e)
    {

      var working = ObjectUtils.Clone(_workingPreferences);
      //  if (working.DataLocation != _originalPreferences.DataLocation)
      //{
        
      //  var to = working.DataLocation;
      //  var from = AppConfig.Default.SaveLocation;
      //  if (String.IsNullOrWhiteSpace(to))
      //  {
      //    to = AppConfig.Default.DefaultSaveLocation;
      //  }

      //  if(String.IsNullOrWhiteSpace(from))
      //  {
      //    from = AppConfig.Default.DefaultSaveLocation;
      //  }

      //  if(to == from)
      //  {
      //    return;
      //  }

      //  JsonConfigTools.CopyDataLocation(from, to, true);

      //  AppConfig.Default.SaveLocation = working.DataLocation;
      //  AppConfig.Default.Save();
      //}

      _originalPreferences.InjectFrom(working);
      _originalPreferences.Save();

      SaveButtonState();

    }

    private void _originalPreferences_Saved(object? sender, Preferences e)
    {
      throw new NotImplementedException();
    }

    private void alwaysOnTop_Checked(object sender, RoutedEventArgs e)
    {

    }

    //private void dataLocationoiButton_Click(object sender, RoutedEventArgs e)
    //{

    //  using (var dialog = new FolderBrowserDialog())
    //  {
    //    dialog.Description = "Select a folder";
    //    dialog.ShowNewFolderButton = true;
    //    dialog.RootFolder = Environment.SpecialFolder.MyComputer;

    //    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
    //    {
    //      string selectedPath = dialog.SelectedPath;
    //      // Use the selected path as needed
    //      dataLocationTextBox.Text = selectedPath;
    //    }
    //  }

    //}

    //private void dataLocationClearButton_Click(object sender, RoutedEventArgs e)
    //{
    //  dataLocationTextBox.Text = "";
    //}
  }
}
