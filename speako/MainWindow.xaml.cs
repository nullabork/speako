using Microsoft.Extensions.DependencyInjection;

using speako.Services.VoiceSettings;
using speako.Settings;
using System.Windows;
using System.Windows.Controls;

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
      //_speak = speak;
      AddVoices();
    }

    private async void AddVoices()
    {
      CancellationTokenSource cts = new CancellationTokenSource();

      _applicationSettings.ConfiguredProviders.ForEach(async provider =>
      {
        //cancellation token
        foreach (var item in (await provider.Provider.GetVoicesAsync(cts.Token)))
        {
          var cbItem = new ComboBoxItem();
          cbItem.Content = item.Name;
          cbItem.DataContext = item;
          voicesComboBox.Items.Add(cbItem);
        }
      });
    }

    private async void speakButton_Click(object sender, RoutedEventArgs e)
    {
      //print line
      System.Diagnostics.Debug.WriteLine("stream:ads asd asd assd asd asd asd asd asd sd  ");
      //await _speak.SpeakText(speakInput.Text);
    }

    private void providersMenuItem_Click(object sender, RoutedEventArgs e)
    {
      var providersSettingsWindow = _serviceProvider.GetRequiredService<ProvidersSettingsWindow>();
      providersSettingsWindow.ShowDialog();

    }

    private void voicesMenuItem_Click(object sender, RoutedEventArgs e)
    {
      var voiceSettingsWindow = _serviceProvider.GetRequiredService<VoiceSettingsWindow>();
      voiceSettingsWindow.ShowDialog();
    }

    //ComboBoxItem_MouseUp
    private void voicesComboBoxItem_MouseUp(object sender, RoutedEventArgs e)
    {
      //var item = (ComboBoxItem)sender;
      //var provider = (ITTSProvider)item.DataContext;
      //var settingsControl = provider.GetSettingsControl();
      //settingsControl.ShowDialog();

    }


  }
}