using speako.Services.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace speako
{
    /// <summary>
    /// Interaction logic for ProvidersSettingsWindow.xaml
    /// </summary>
    public partial class ProvidersSettingsWindow : Window
    {
    public ProvidersSettingsWindow(IEnumerable<ITTSProvider> providers)
    {
      InitializeComponent();
      providerComboBox.ItemsSource = providers.Select(p => p.Name).ToList();
      providerComboBox.SelectedIndex = 0;  // Optionally set the default selected index
    }

    private void addProviderButton_Click(object sender, RoutedEventArgs e)
    {

    }
  }
}
