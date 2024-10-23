using Microsoft.Extensions.DependencyInjection;

using speako.Services.VoiceProfiles;
using speako.Settings;
using System.Windows;
using System.Windows.Controls;
using speako.Services.ProviderSettings;
using speako.Services.PostProcessors;
using speako.Services.Speak;
using System.Windows.Input;
using System.Windows.Forms;
using Amazon;
using speako.Services.PreProcessors;
using speako.Services.Providers.Piper;
using PiperSharp.Models;
using PiperSharp;

namespace speako
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private readonly IServiceProvider _serviceProvider;
    private readonly ApplicationSettings _applicationSettings;
    private readonly SessionService _sessionService;
    private readonly Preferences _preferences;
    private readonly SpeakService _speakService;

    public MainWindow(IServiceProvider serviceProvider, ApplicationSettings applicationSettings, SessionService ss, Preferences preferences, SpeakService speakService)
    {
      InitializeComponent();
      _serviceProvider = serviceProvider;
      _applicationSettings = applicationSettings;
      _sessionService = ss;
      _preferences = preferences;
      _speakService = speakService;

      voiceProfileComboBox.DataContext = _applicationSettings;
      UpdateMessageHistory();
    }

    private void SendMessage(string message, VoiceProfile vp)
    {

      bool isAtBottom = IsScrolledToBottom();

      if (!string.IsNullOrWhiteSpace(message))
      {

        _speakService.SpeakText(message, vp);
        _sessionService.AddMessage(message, vp);

        messageTextBlock.Clear();

        if (isAtBottom)
        {
          ScrollToBottom();
        }

      }
    }
    public void UpdateMessageHistory()
    {
      messageList.DataContext = _sessionService;
      ScrollToBottom();
    }

    private void providersMenuItem_Click(object sender, RoutedEventArgs e)
    {
      var window = _serviceProvider.GetRequiredService<ProvidersListWindow>();
      window.ShowDialog();
    }

    private void voicesMenuItem_Click(object sender, RoutedEventArgs e)
    {
      var window = _serviceProvider.GetRequiredService<VoiceProfilesListWindow>();
      window.ShowDialog();
    }

    private void preferencesMenuItem_Click(object sender, RoutedEventArgs e)
    {
      var window = _serviceProvider.GetRequiredService<PreferencesWindow>();
      window.ShowDialog();
    }

    private void postProcessorsMenuItem_Click(object sender, RoutedEventArgs e)
    {
      var window = _serviceProvider.GetRequiredService<PostProcessorListingWindow>();
      window.ShowDialog();
    }

    private void preProcessorsMenuItem_Click(object sender, RoutedEventArgs e)
    {
      var window = _serviceProvider.GetRequiredService<PreProcessorListingWindow>();
      window.ShowDialog();
    }

    private void messageButton_Click(object sender, RoutedEventArgs e)
    {
      var profile = (VoiceProfile)voiceProfileComboBox.SelectedItem;
      SendMessage(messageTextBlock.Text, profile);
      //speako();
    }

    private void messageTextBlock_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if (e.Key == Key.Enter && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift))
      {
        // Suppress the Enter key to prevent new line from being added
        e.Handled = true;

        // Call the send function
        var profile = (VoiceProfile)voiceProfileComboBox.SelectedItem;
        SendMessage(messageTextBlock.Text, profile);
      }
    }


    private bool IsScrolledToBottom()
    {
      // Check if the ScrollViewer is at the bottom
      return messageList.VerticalOffset >= messageList.ScrollableHeight;
    }

    private void ScrollToBottom()
    {
      // Scroll the ScrollViewer to the bottom
      messageList.ScrollToEnd();
    }

    private void copyMenuItem_Click(object sender, RoutedEventArgs e)
    {
      var menuItem = (MenuItem)sender;
      var message = (TextMessage)menuItem.DataContext;

      System.Windows.Clipboard.SetText(message.AsString());

    }

    private void resendMenuItem_Click(object sender, RoutedEventArgs e)
    {
      var menuItem = (MenuItem)sender;
      var message = (TextMessage)menuItem.DataContext;
      var vp = _applicationSettings.ConfiguredVoices.FirstOrDefault(Profile => Profile.Name == message.VoiceProfileName, null);
      SendMessage(message.Message, vp);
    }

    private void copySessionMenuItem_Click(object sender, RoutedEventArgs e)
    {
      var menuItem = (MenuItem)sender;
      var session = (MessageSession)menuItem.DataContext;
      System.Windows.Clipboard.SetText(session.AsString());
    }

    private void copyMessageMenuItem_Click(object sender, RoutedEventArgs e)
    {
      var menuItem = (MenuItem)sender;
      var session = (TextMessage)menuItem.DataContext;
      System.Windows.Clipboard.SetText(session.Message);
    }

    private async void speako()
    {

      string eSpeakDataPath = "path_to_eSpeak_data";
      string modelPath = "C:\\Users\\aaron\\OneDrive\\Documents\\voice";
      string modelConfigPath = "C:\\Users\\aaron\\OneDrive\\Documents\\voice\\en_GB-alan-medium.onnx.json";
      string text = "Hello, world! Hello, world! Hello, world! Hello, world!";
      string outputAudioFile = "output.wav";


      var cwd = Directory.GetCurrentDirectory();

      //check for piper/piper.exe 
      if (!File.Exists(Path.Join(cwd, "piper", "piper.exe")))
      {
        await PiperDownloader.DownloadPiper().ExtractPiper(cwd);
      }
      // Downloads and extracts piper to cwd/piper directory

      //// You can get list of models from hugging face using
      //      var models = await PiperDownloader.GetHuggingFaceModelList(); // Returns a dictionary with model key as key
      //      var model = models["ar_JO-kareem-low"];

      //      // or you can do
      var model = await PiperDownloader.GetModelByKey("en_GB-alan-medium");

      // Before you can use the model you need to download it using
      model = await model.DownloadModel(cwd);
      // Or if its downloaded you can load it from directory

      //var model = await VoiceModel.LoadModel(modelPath);

      // Now you can also do
      //model = await PiperDownloader.DownloadModelByKey("ar_JO-kareem-low");
      // Or if its downloaded you can load it by key aswell
      //model = await VoiceModel.LoadModelByKey("ar_JO-kareem-low");

      // To start generating audio use PiperProvider
      var piperModel = new PiperProvider(new PiperConfiguration()
      {
        ExecutableLocation = Path.Join(cwd, "piper", "piper.exe"), // Path to piper executable
        WorkingDirectory = Path.Join(cwd, "piper"), // Path to piper working directory
        Model = model, // Loaded/downloaded VoiceModel
      });

      // Generate audio, currently supported formats are Mp3, Wav, Raw
      var result = await piperModel.InferAsync(text, AudioOutputType.Mp3);


      using var stream = new MemoryStream(result);

      var profile = (VoiceProfile)voiceProfileComboBox.SelectedItem;
      SendMessage(messageTextBlock.Text, profile);


    }

    private void loadMoreButton_Click(object sender, RoutedEventArgs e)
    {
      _sessionService.LoadNext();
    }


  }
}