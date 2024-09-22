
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using speako.Settings;
using speako.Services.Providers;
using speako.Services.Providers.AWS;
using speako.Services.Providers.ElevenLabs;
using speako.Services.Providers.Google;
using speako.Services.Speak;
using speako.Services.ProviderSettings;
using speako.Services.VoiceSettings;
using speako.Common;
using speako.Services.Audio;

namespace speako
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application 
  {
    public IServiceProvider ServiceProvider { get; private set; }
    
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      var serviceCollection = new ServiceCollection();
      ConfigureServices(serviceCollection);

      ServiceProvider = serviceCollection.BuildServiceProvider();

      var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
      mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {

      var a = ApplicationSettings.Load();
      services.AddSingleton(a);
      services.AddSingleton<IAudioService, AudioService>();
      services.AddSingleton<VoiceQueue>();
      services.AddSingleton<AudioDevicesService>();

      services.AddTransient<MainWindow>();
      services.AddTransient<ProvidersSettingsWindow>();
      services.AddTransient<VoiceProfilesListWindow>();
      services.AddTransient<VoiceProfileDetailWindow>();
      services.AddTransient<SpeakService>();
      services.AddTransient<VoiceProfileSpeakControl>();      


      services.AddTransient<ITTSProvider, GoogleTTSProvider>();
      services.AddTransient<ITTSProvider, AWSTTSProvider>();
      services.AddTransient<ITTSProvider, ElevenLabsTTSProvider>();
    }
  }
}
