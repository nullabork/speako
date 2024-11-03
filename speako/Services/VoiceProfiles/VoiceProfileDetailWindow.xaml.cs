using speako.Common;
using speako.Services.Speak;
using speako.Settings;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using speako.Services.Auth;
using System.ComponentModel;
using speako.Services.Audio;
using System.Collections.ObjectModel;
using speako.Services.PostProcessors;
using speako.Services.PreProcessors;

namespace speako.Services.VoiceProfiles
{
  public partial class VoiceProfileDetailWindow : Window, IDisposable
  {
    private readonly ApplicationSettings _applicationSettings;
    private readonly SpeakService _speakService;
    public event EventHandler<VoiceProfile> Saved;
    private VoiceProfile _originalVoiceProfile;
    private VoiceProfile _workingVoiceProfile;
    private readonly AudioDevicesService _audioDevicesService;

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
      postProcessorsComboBox.ItemsSource = applicationSettings.PostProcessors.Select(pp => new PostProcessorItem { ProcessorGuid = pp.GUID, ProcessorName = pp.Name });
      preProcessorsComboBox.ItemsSource = applicationSettings.PreProcessors.Select(pp => new PreProcessorItem { ProcessorGuid = pp.GUID, ProcessorName = pp.Name });
      providerComboBox.ItemsSource = _applicationSettings.ProviderSettings;

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

      if (configured?.GetProvider() == null) return;

      var provider = configured.GetProvider();
      var voices = await provider.GetVoicesAsync(default);
      voiceComboBox.ItemsSource = voices;
      voiceComboBox.SelectedIndex = voices.ToList().FindIndex(item =>
      {
        return item.Id == selectedVoiceID;
      });
    }

    public void ConfigureVoice(VoiceProfile? voice)
    {
      _originalVoiceProfile = voice;
      _workingVoiceProfile = ObjectUtils.Clone(voice);
      _workingVoiceProfile.AudioDevices = new ObservableCollection<AudioDevice>(_originalVoiceProfile.AudioDevices);
      _workingVoiceProfile.PostProcessors = new ObservableCollection<PostProcessorItem>(_originalVoiceProfile.PostProcessors);
      SaveButtonState();
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

    private void saveButton_Click(object sender, RoutedEventArgs e)
    {
      if (Saved != null)
      {
        //_originalVoiceProfile = _workingVoiceProfile;
        //_workingVoiceProfile = ObjectUtils.Clone(_workingVoiceProfile);
        //_workingVoiceProfile.AudioDevices = new ObservableCollection<AudioDevice>(_originalVoiceProfile.AudioDevices);
        //_workingVoiceProfile.PostProcessors = new ObservableCollection<PostProcessorItem>(_originalVoiceProfile.PostProcessors);

        //DataContext = _workingVoiceProfile;
        _originalVoiceProfile = ObjectUtils.Clone(_workingVoiceProfile);

        Saved.Invoke(this, _originalVoiceProfile);
        SaveButtonState();
      }
    }

    private async void TestButton_Click(object sender, RoutedEventArgs e)
    {

      await _speakService.SpeakText(ttsTestSentence.Text, _workingVoiceProfile);
    }

    private void volumeValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      var t = "Asd";
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
      var profile = settings?.GetProvider()?.DefaultVoiceProfile();
      if (profile != null)
      {
        pitchValueSlider.Value = profile.Pitch;
      }
    }

    private void ResetSpeed()
    {
      var settings = (IAuthSettings)providerComboBox.SelectedItem;
      var profile = settings?.GetProvider()?.DefaultVoiceProfile();
      if (profile != null)
      {
        speedValueSlider.Value = profile.Speed;
      }
    }

    private void ResetVolume()
    {
      var settings = (IAuthSettings)providerComboBox.SelectedItem;
      var profile = settings?.GetProvider()?.DefaultVoiceProfile();
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
        var index = _workingVoiceProfile.AudioDevices.ToList().FindIndex(d => string.Equals(d.DeviceGuid, device.DeviceGuid));
        _workingVoiceProfile.AudioDevices.RemoveAt(index);
        SaveButtonState();
      }
    }

    private void addProcessor_Click(object sender, RoutedEventArgs e)
    {
      var pp =  (PostProcessorItem)postProcessorsComboBox.SelectedItem;
      if(pp != null)
      {
        _workingVoiceProfile.PostProcessors.Add(pp);
        SaveButtonState();
      }
    }

    private void delProcessor_Click(object sender, RoutedEventArgs e)
    {
      var selectedPP = (PostProcessorItem)selectedProcessors.SelectedItem;
      if (selectedPP != null)
      {
        var index = _workingVoiceProfile.PostProcessors.ToList().FindIndex(pp => string.Equals(pp.ProcessorGuid, selectedPP.ProcessorGuid));
        _workingVoiceProfile.PostProcessors.RemoveAt(index);
        SaveButtonState();
      }
    }

    private void addPreProcessor_Click(object sender, RoutedEventArgs e)
    {
      var pp = (PreProcessorItem)preProcessorsComboBox.SelectedItem;
      if (pp != null)
      {
        _workingVoiceProfile.PreProcessors.Add(pp);
        SaveButtonState();
      }
    }

    private void delPreProcessor_Click(object sender, RoutedEventArgs e)
    {
      var selectedPP = (PreProcessorItem)selectedPreProcessors.SelectedItem;
      if (selectedPP != null)
      {
        var index = _workingVoiceProfile.PreProcessors.ToList().FindIndex(pp => string.Equals(pp.ProcessorGuid, selectedPP.ProcessorGuid));
        _workingVoiceProfile.PreProcessors.RemoveAt(index);
        SaveButtonState();
      }
    }
  }
}