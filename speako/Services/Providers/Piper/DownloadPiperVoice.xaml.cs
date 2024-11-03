
using PiperSharp;
using speako.Common;
using speako.Services.Auth;
using System.Windows;
using System.Windows.Controls;

namespace speako.Services.Providers.Piper
{
  /// <summary>
  /// Interaction logic for DownloadPiperVoice.xaml
  /// </summary>
  public partial class DownloadPiperVoice : Window
  {

    public event EventHandler<PiperTTSVoice?>? Downloaded;

    private PiperTTSVoice _voice { set; get; }
    public DownloadPiperVoice(PiperTTSVoice voice)
    {
      InitializeComponent();
      DataContext = voice;
      _voice = voice;
    }

    //TODO better workflow and error handling
    public async Task Download()
    {
     
      var model = await PiperDownloader.GetModelByKey(_voice.Id);

      if (!PiperTTSProvider.ModelIsDownloaded(_voice.Id))
      {
        var path = JsonConfigTools.GetDataDirectory();

        

        //initialContent.Visibility = Visibility.Collapsed;
        //finishedContent.Visibility = Visibility.Visible;
        //Downloaded?.Invoke(_voice, _voice);

        // Marshal back to the UI thread to update UI elements
        //Dispatcher.Invoke(() =>
        //{
        //  initialContent.Visibility = Visibility.Collapsed;
        //  finishedContent.Visibility = Visibility.Visible;
        //  Downloaded?.Invoke(_voice, _voice);
        //});

        try
        {
          await model.DownloadModel(Path.Join(path, "piper"));

          initialContent.Visibility = Visibility.Collapsed;
          finishedContent.Visibility = Visibility.Visible;

          Downloaded?.Invoke(_voice, _voice);
        }
        catch
        {
          //Dispatcher.Invoke(() =>
          //{
            initialContent.Visibility = Visibility.Collapsed;
            finishedContent.Visibility = Visibility.Collapsed;
            errorContent.Visibility = Visibility.Visible;
            Downloaded?.Invoke(null, null);
          //});
        }
        finally
        {
          //Dispatcher.Invoke(() =>
          //{
            
          //});
        }

      }
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      await Download();
    }
  }
}
