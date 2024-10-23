using Omu.ValueInjecter;
using speako.Common;
using speako.Services.Audio;
using System.ComponentModel;
using System.Windows;
using speako.Themes;

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


      _originalPreferences.InjectFrom(ObjectUtils.Clone(_workingPreferences));
      _originalPreferences.Save();
      
      //DataContext = _workingPreferences;

      SaveButtonState();

    }

    private void _originalPreferences_Saved(object? sender, Preferences e)
    {
      throw new NotImplementedException();
    }

    private void alwaysOnTop_Checked(object sender, RoutedEventArgs e)
    {

    }
  }
}
