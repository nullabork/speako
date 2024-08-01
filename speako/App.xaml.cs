using System.Configuration;
using System.Data;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using speako.Features.Speak;
using speako.Features.Speak.Providers;
using speako.Google;

namespace speako
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private IHost _host;

    /// <summary>
    /// Initializes a new instance of the App class.
    /// </summary>
    public App()
    {
      _host = Host.CreateDefaultBuilder()
          .ConfigureServices((context, services) =>
          {
            // Register services
            services.AddSingleton<MainWindow>();

            services.AddTransient<ISpeakService, SpeakService>();

            services.AddTransient<ITTSProvider, GoogleTTSProvider>();
            services.AddTransient<ITTSProvider, AWSTTSProvider>();
            services.AddTransient<ITTSProvider, IBMTTSProvider>();
            services.AddTransient<ITTSProvider, AzureTTSProvider>();
            services.AddTransient<ITTSProvider, ElevenLabsTTSProvider>();

          })
          .Build();
    }

    /// <summary>
    /// Handles the startup event of the application.
    /// </summary>
    /// <param name="e">Startup event arguments.</param>
    protected override async void OnStartup(StartupEventArgs e)
    {
      await _host.StartAsync();
      var mainWindow = _host.Services.GetRequiredService<MainWindow>();
      mainWindow.Show();
      base.OnStartup(e);
    }

    /// <summary>
    /// Handles the exit event of the application.
    /// </summary>
    /// <param name="e">Exit event arguments.</param>
    protected override async void OnExit(ExitEventArgs e)
    {
      await _host.StopAsync();
      _host.Dispose();
      base.OnExit(e);
    }
  }

}
