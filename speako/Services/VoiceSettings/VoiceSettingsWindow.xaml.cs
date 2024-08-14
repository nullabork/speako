using Microsoft.Extensions.DependencyInjection;
using speako.Settings;
using System.Windows;

namespace speako.Services.VoiceSettings
{
  /// <summary>
  /// Interaction logic for VoiceSettingsWindow.xaml
  /// </summary>
  public partial class VoiceSettingsWindow : Window
  {
    private readonly VoiceWindow _serviceProvider;
    private readonly VoiceWindow voiceWindow;
    private readonly ApplicationSettings _applicationSettings;

    public VoiceSettingsWindow(VoiceWindow voiceWindow, ApplicationSettings applicationSettings)
    {
      InitializeComponent();
      this.voiceWindow = voiceWindow;
      _applicationSettings = applicationSettings;
    }

    private void AddVoiceButton_Click(object sender, RoutedEventArgs e)
    {
     
      var cv = new ConfiguredVoice();
      voiceWindow.DataContext = cv;
      voiceWindow.ShowDialog();
    }
  }
}
