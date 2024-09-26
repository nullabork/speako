using Amazon.Runtime.Internal.Util;
using NAudio.Wave;
using speako.Common;
using speako.Services.Speak;
using speako.Settings;
using System.Windows;
using System.Windows.Controls;


using System.Windows.Input;
using speako.Services.Auth;
using System.ComponentModel;
using Omu.ValueInjecter;
using speako.Services.Audio;
using Google.Protobuf.WellKnownTypes;
using System.Collections.ObjectModel;

namespace speako.Services.VoiceSettings
{

  /// <summary>
  /// Interaction logic for VoiceWindow.xaml
  /// </summary>
  public partial class VoiceProfileDetailWindow : Window, IDisposable
  {
    private readonly ApplicationSettings _applicationSettings;
    private readonly SpeakService _speakService;
    public event EventHandler<VoiceProfile> Saved;
    private VoiceProfile _originalVoiceProfile;
    private VoiceProfile _workingVoiceProfile;
    private readonly AudioDevicesService _audioDevicesService;

    //TODO remove this its stupid
    //public VoiceProfile VoiceContext
    //{
    //  set
    //  {
    //    DataContext = value;
    //    value.PropertyChanged += OnPropertyChanged;
    //  }
    //  get {

    //    if (DataContext == null)
    //    {
    //      DataContext = new VoiceProfile();
    //    }

    //    return (VoiceProfile)DataContext;
    //  }
    //}

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      SaveButtonState();
    }

    public VoiceProfileDetailWindow(ApplicationSettings applicationSettings, SpeakService speak, AudioDevicesService devices)
    {
      InitializeComponent();

      _applicationSettings = applicationSettings;
      _speakService = speak;
      _audioDevicesService = devices;
      outputDeviceComboBox.ItemsSource = devices.AudioDevices.Values;
      providerComboBox.ItemsSource = _applicationSettings.ConfiguredProviders;

      LoadProviderVoicesAsync(null);
    }

    private void SaveButtonState()
    {
      var isEqual = Compare.ObjectsPropertiesEqual(_workingVoiceProfile, _originalVoiceProfile);

      saveButton.IsEnabled = !isEqual;
    }

    private async Task LoadProviderVoicesAsync(string? selectedVoiceID)
    {
      var configured = (IAuthSettings)providerComboBox.SelectedItem;

      if (configured == null) return;

      if (configured?.Provider == null) return;

      var voices = await configured.Provider.GetVoicesAsync(default);
      voiceComboBox.ItemsSource = voices;
      voiceComboBox.SelectedIndex = voices.ToList().FindIndex(item =>
      {
        return item.Name == selectedVoiceID;
      });
    }

    public void ConfigureVoice(VoiceProfile? voice)
    {
      _originalVoiceProfile = voice;
      _workingVoiceProfile = ObjectUtils.Clone(voice);
      _workingVoiceProfile.AudioDevices = new ObservableCollection<AudioDevice>(_originalVoiceProfile.AudioDevices);
      _workingVoiceProfile.PropertyChanged += OnPropertyChanged;

      DataContext = _workingVoiceProfile;

      this.Title = string.IsNullOrEmpty(_workingVoiceProfile?.Name) ? "New Voice" : $"Edit {_workingVoiceProfile.Name}";

      if (!string.IsNullOrEmpty(_workingVoiceProfile.ConfiguredProviderGUID))
      {
        providerComboBox.SelectedValue = _workingVoiceProfile.ConfiguredProviderGUID;
      }

      //TODO this is dumb also, change it ....
      if (!string.IsNullOrEmpty(_workingVoiceProfile.VoiceID))
      {
        //well i actually 
        LoadProviderVoicesAsync(_workingVoiceProfile.VoiceID);
      }
    }

    private void outputDeviceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var configured = (AudioDevice)e.AddedItems[0];
      _workingVoiceProfile.DeviceName = configured.DeviceName;
    }

    private void saveButton_Click(object sender, RoutedEventArgs e)
    {
      if (Saved != null)
      {
        _originalVoiceProfile = _workingVoiceProfile;
        _workingVoiceProfile = ObjectUtils.Clone(_workingVoiceProfile);
        _workingVoiceProfile.AudioDevices = new ObservableCollection<AudioDevice>(_originalVoiceProfile.AudioDevices);

        DataContext = _workingVoiceProfile;

        Saved.Invoke(this, _workingVoiceProfile);
        SaveButtonState();
      }
    }

    private async void TestButton_Click(object sender, RoutedEventArgs e)
    {
      var settings = (IAuthSettings)providerComboBox.SelectedItem;
      await _speakService.SpeakText(settings.Provider, _workingVoiceProfile, ttsTestSentence.Text);
    }

    private void volumeValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {

    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!Compare.ObjectsPropertiesEqual(_workingVoiceProfile, _originalVoiceProfile))
      {
        MessageBoxResult result = MessageBox.Show($"Do you want to save your changes?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
          Saved.Invoke(this, _workingVoiceProfile);
        }
      }
    }

    private async void providerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      await LoadProviderVoicesAsync(_workingVoiceProfile.VoiceID);
      ResetPitch();
      ResetSpeed();
      ResetVolume();
    }

    private void ResetPitch()
    {
      var settings = (IAuthSettings)providerComboBox.SelectedItem;
      var profile = settings?.Provider?.DefaultVoiceProfile();
      if (profile != null)
      {
        pitchValueSlider.Value = profile.Pitch;
      }
    }

    private void ResetSpeed()
    {
      var settings = (IAuthSettings)providerComboBox.SelectedItem;
      var profile = settings?.Provider?.DefaultVoiceProfile();
      if (profile != null)
      {
        speedValueSlider.Value = profile.Speed;
      }
    }

    private void ResetVolume()
    {
      var settings = (IAuthSettings)providerComboBox.SelectedItem;
      var profile = settings?.Provider?.DefaultVoiceProfile();
      if (profile != null)
      {
        volumeValueSlider.Value = profile.Volume;
      }
    }

    private void resetPitch_MouseDown(object sender, MouseButtonEventArgs e)
    {
      ResetPitch();
    }

    private void resetSpeed_MouseDown(object sender, MouseButtonEventArgs e)
    {
      ResetSpeed();
    }

    private void resetVolume_MouseDown(object sender, MouseButtonEventArgs e)
    {
      ResetVolume();
    }

    public void Dispose()
    {
      Saved = null;
    }

    private void Window_Closed(object sender, EventArgs e)
    {
      Dispose();
    }

    private void cancelButton_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void addDeviceButton_click(object sender, RoutedEventArgs e)
    {
      var device = (AudioDevice)outputDeviceComboBox.SelectedItem;
      if (device != null)
      {
        _workingVoiceProfile.AudioDevices.Add(device);
        SaveButtonState();
      }
    }

    private void delDeviceButton_click(object sender, RoutedEventArgs e)
    {
      var device = (AudioDevice)selectedDevices.SelectedItem;
      if (device != null)
      {
        _workingVoiceProfile.AudioDevices.Remove(device);
        SaveButtonState();
      }
    }
  }
}