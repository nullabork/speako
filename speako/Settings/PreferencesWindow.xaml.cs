using Omu.ValueInjecter;
using speako.Common;
using speako.Services.Audio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace speako.Settings
{
  /// <summary>
  /// Interaction logic for PreferencesWindow.xaml
  /// </summary>
  public partial class PreferencesWindow : Window
  {
    private readonly AudioDevicesService _AudioDevicesService;
    private Preferences _preferences;
    private Preferences _originalPreferences;
    public PreferencesWindow(AudioDevicesService devices, Preferences preferences)
    {
      InitializeComponent();
      _originalPreferences = preferences;
      _preferences = ObjectUtils.Clone(preferences);

      _AudioDevicesService = devices;
      outputDeviceComboBox.ItemsSource = devices.AudioDevices.Values;
      

      DataContext = _preferences;
      _preferences.PropertyChanged += OnPropertyChanged;
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      var isEqual = Compare.ObjectsPropertiesEqual(_originalPreferences, _preferences);
      saveButton.IsEnabled = !isEqual;
    }

    private void outputDeviceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void cancelButton_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void saveButton_Click(object sender, RoutedEventArgs e)
    {
      _originalPreferences.InjectFrom(_preferences);
      _originalPreferences.Save();
    }
  }
}
