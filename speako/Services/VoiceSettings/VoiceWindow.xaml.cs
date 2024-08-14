using speako.Settings;
using System.Windows;
using System.Windows.Controls;

namespace speako.Services.VoiceSettings
{
  /// <summary>
  /// Interaction logic for VoiceWindow.xaml
  /// </summary>
  public partial class VoiceWindow : Window
  {
    private readonly ApplicationSettings _applicationSettings;
    private readonly IServiceProvider _serviceProvider;

    public VoiceWindow(IServiceProvider serviceProvider, ApplicationSettings applicationSettings)
    {
      InitializeComponent();
      _applicationSettings = applicationSettings;
      _serviceProvider = serviceProvider;

      outputDeviceComboBox.ItemsSource = Common.AudioDevice.GetAudioDevices();
      providerComboBox.ItemsSource = _applicationSettings.ConfiguredProviders;

    }
    private void TestButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private async void  providerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var configured = (ConfiguredProvider)providerComboBox.SelectedItem;
      CancellationTokenSource cts = new CancellationTokenSource();
      voiceComboBox.ItemsSource = await configured.Provider.GetVoicesAsync(cts.Token);
    }
  }
}
