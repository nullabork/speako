
using speako.Services.Providers.Google;
using speako.Services.Speak;
using System.Windows;

namespace speako
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    //private readonly ISpeakService _speak;

    public MainWindow()
    {
      InitializeComponent();
      //_speak = speak;
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {

    }

    private void googleTTSMenuItem_Click(object sender, RoutedEventArgs e)
    {
      var settingsWindow = new SettingsWindow();
      var GoogleSettingsControl = new GoogleSettingsControl();
      settingsWindow.settingsScroller.Content = GoogleSettingsControl;
      settingsWindow.ShowDialog();
    }

    private async void speakButton_Click(object sender, RoutedEventArgs e)
    {
      //print line
      System.Diagnostics.Debug.WriteLine("stream:ads asd asd assd asd asd asd asd asd sd  ");
      //await _speak.SpeakText(speakInput.Text);
    }
  }
}