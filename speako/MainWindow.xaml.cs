using Microsoft.Extensions.DependencyInjection;

using speako.Services.VoiceSettings;
using speako.Settings;
using System.Windows;
using System.Windows.Controls;
using speako.Services.ProviderSettings;

namespace speako
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private readonly IServiceProvider _serviceProvider;
    private readonly ApplicationSettings _applicationSettings;

    public MainWindow(IServiceProvider serviceProvider, ApplicationSettings applicationSettings)
    {
      InitializeComponent();
      _serviceProvider = serviceProvider;
      _applicationSettings = applicationSettings;
      CreateVoiceTabs();
    }

    public void CreateVoiceTabs()
    {
      var profiles = _applicationSettings.ConfiguredVoices.ToList();
      profiles.ForEach(p =>
      {
        var content = _serviceProvider.GetRequiredService<VoiceProfileSpeakControl>();
        content.VoiceContext = p;

        voiceProfileTabs.Items.Add(new TabItem
        {
          Header = p.Name,
          Content = content,
        });
      });
    }

    private void providersMenuItem_Click(object sender, RoutedEventArgs e)
    {
      var window = _serviceProvider.GetRequiredService<ProvidersSettingsWindow>();
      window.ShowDialog();
    }

    private void voicesMenuItem_Click(object sender, RoutedEventArgs e)
    {
      var window = _serviceProvider.GetRequiredService<VoiceProfilesListWindow>();
      window.ShowDialog();
    }
  }
}