
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using speako.Settings;
using speako.Services.Providers.Google;
using speako.Services.Speak;
using speako.Services.ProviderSettings;
using speako.Services.VoiceProfiles;
using speako.Common;
using speako.Services.Audio;
using speako.Services.PostProcessors;
using speako.Services.PostProcessors.Discord;
using speako.Services.Auth;
using speako.Themes;
using System.Xml.Serialization;
using speako.Services.PreProcessors;
using speako.Services.PreProcessors.TextReplacer;
using speako.Services.PostProcessors.DiscordWebHook;
using speako.Services.Providers.Piper;

namespace speako
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application 
  {
    public IServiceProvider ServiceProvider { get; private set; }
    private Preferences preferences;

    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      var serviceCollection = new ServiceCollection();
      ConfigureServices(serviceCollection);
    }


    private async void ConfigureServices(IServiceCollection services)
    {
   

      preferences = new Preferences();
      preferences.Saved += Pref_Saved;
      //preferences.Loaded += Pref_Loaded;
      await preferences.Load();
      services.AddSingleton(preferences);

      UpdatePreferences(preferences);
      UpdateTheme(preferences);

      var app = new ApplicationSettings();
      await app.Load();

      services.AddSingleton(app);

      services.AddTransient<MainWindow>();


      //singleton services
      services.AddSingleton<IAudioService, AudioService>();
      services.AddSingleton<VoiceQueue>();
      services.AddSingleton<AudioDevicesService>();

      var ss = new SessionService();
      await ss.LoadNext(3);
      services.AddSingleton(ss);

      //services
      services.AddTransient<SpeakService>();

      //windows
      
      services.AddTransient<ProvidersListWindow>();
      services.AddTransient<VoiceProfilesListWindow>();
      services.AddTransient<VoiceProfileDetailWindow>();
      services.AddTransient<PreferencesWindow>();
      services.AddTransient<PostProcessorListingWindow>();
      services.AddTransient<PostProcessorDetailWindow>();
      services.AddTransient<PreProcessorListingWindow>();
      services.AddTransient<PreProcessorDetailWindow>();

      //Providers
      services.AddTransient<IAuthSettings, GoogleAuthSettings>();
      services.AddTransient<IAuthSettings, PiperTTSAuthSettings>();
      //services.AddTransient<ITTSProvider, ElevenLabsTTSProvider>();

      //PreProcessors 
      services.AddTransient<IPreProcessorSettings, TextReplacerProcessorSettings>();

      //PostProcessors 
      services.AddTransient<IPostProcessorSettings, DiscordProcessorSettings>();
      services.AddTransient<IPostProcessorSettings, DiscordWebHookProcessorSettings>();

      ServiceProvider = services.BuildServiceProvider();

      var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
      mainWindow.Show();


      EventManager.RegisterClassHandler(typeof(Window), Window.LoadedEvent, new RoutedEventHandler(Window_Loaded));
    }
    private void UpdatePreferences(Preferences pref)
    {
      //if (pref.AlwaysOnTop != Application.Current.MainWindow.Topmost) {  
        foreach (Window window in Windows)
        {
          window.Topmost = pref.AlwaysOnTop;
        }
      //}
    }

    private void UpdateTheme(Preferences pref)
    {
      if (pref.Theme != null)
      {
        //ThemeType theme = Enum.Parse<ThemeType>(pref.Theme);
        var found = Enum.TryParse<ThemeType>(pref.Theme, out ThemeType theme);
        if (found)
        {
          ThemesController.SetTheme(theme);
        }
      }
    }

    private void Pref_Loaded(object? sender, Preferences e)
    {

    }
    
    private void Pref_Saved(object? sender, Preferences e)
    {
      UpdatePreferences(e);
      UpdateTheme(e);
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Window window = sender as Window;
      if (window != null && preferences != null)
      {
        UpdatePreferences(preferences);
      }
    }
  }
}
