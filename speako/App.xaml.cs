using System.Configuration;
using System.Data;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using speako.Services.Providers;
using speako.Services.Providers.AWS;
using speako.Services.Providers.Azure;
using speako.Services.Providers.ElevenLabs;
using speako.Services.Providers.Google;
using speako.Services.Providers.IBM;
using speako.Services.Speak;

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
      // Register your services and windows here
      services.AddTransient<MainWindow>();
      services.AddTransient<ProvidersSettingsWindow>();

      services.AddTransient<ISpeakService, SpeakService>();

      services.AddTransient<ITTSProvider, GoogleTTSProvider>();
      services.AddTransient<ITTSProvider, AWSTTSProvider>();
      //services.AddTransient<ITTSProvider, IBMTTSProvider>();
      //services.AddTransient<ITTSProvider, AzureTTSProvider>();
      services.AddTransient<ITTSProvider, ElevenLabsTTSProvider>();
      // Add other services as needed
    }


  }

}
