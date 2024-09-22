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
    private readonly AudioDevicesService _audioDevicesService;

    public VoiceProfile VoiceContext
    {
      set
      {
        DataContext = value;
        value.PropertyChanged += OnPropertyChanged;
      }
      get {

        if (DataContext == null)
        {
          DataContext = new VoiceProfile();
        }

        return (VoiceProfile)DataContext;
      }
    }

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
      var isEqual = Compare.AreObjectsEqual(VoiceContext, _originalVoiceProfile);

      saveButton.IsEnabled = !isEqual;
    }

    private async Task LoadProviderVoicesAsync(string? selectedVoiceID)
    {
      var configured = (IAuthSettings)providerComboBox.SelectedItem;

      if (configured == null) return;

      if(configured?.Provider == null) return;
      
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
      VoiceContext = ObjectUtils.Clone(voice);

      this.Title = string.IsNullOrEmpty(VoiceContext?.Name) ? "New Voice" : $"Edit {VoiceContext.Name}";

      if (!string.IsNullOrEmpty(VoiceContext.ConfiguredProviderGUID))
      {
        providerComboBox.SelectedValue = VoiceContext.ConfiguredProviderGUID;
      }

      if (!string.IsNullOrEmpty(VoiceContext.VoiceID))
      {
        LoadProviderVoicesAsync(VoiceContext.VoiceID);
      }
    }

    private void outputDeviceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var configured = (AudioDevice) e.AddedItems[0];
      VoiceContext.DeviceName = configured.DeviceName;
    }

    private void saveButton_Click(object sender, RoutedEventArgs e)
    {
      if(Saved != null) {
        _originalVoiceProfile.InjectFrom(VoiceContext);
        Saved.Invoke(this, VoiceContext);
        SaveButtonState();
      }
    }
    
    private async void TestButton_Click(object sender, RoutedEventArgs e)
    {
      var settings = (IAuthSettings)providerComboBox.SelectedItem;
      await _speakService.SpeakText(settings.Provider, VoiceContext, ttsTestSentence.Text);
    }

    private void volumeValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {

    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!Compare.AreObjectsEqual(VoiceContext, _originalVoiceProfile))
      {
        MessageBoxResult result = MessageBox.Show($"Do you want to save your changes?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
          Saved.Invoke(this, VoiceContext);
        }
      }
    }

    private async void providerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      await LoadProviderVoicesAsync(VoiceContext.VoiceID);
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
  }
}