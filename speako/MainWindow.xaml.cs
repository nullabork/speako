using speako.Google;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            await Providers.Provider.SpeakText(speakInput.Text);
        }
    }
}