
using System.Windows;

namespace speako
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void googleTTSMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            var GoogleSettingsControl = new Google.GoogleSettingsControl();
            settingsWindow.settingsScroller.Content = GoogleSettingsControl;
            settingsWindow.ShowDialog();
        }

        private async void speakButton_Click(object sender, RoutedEventArgs e)
        {
            //print line
            System.Diagnostics.Debug.WriteLine("stream:ads asd asd assd asd asd asd asd asd sd  ");
            await speako.Common.AudioDevice.SpeakText(speakInput.Text);
        }
    }
}