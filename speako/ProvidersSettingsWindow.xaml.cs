using speako.Services.Providers;
using speako.Settings;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace speako
{
  public partial class ProvidersSettingsWindow : Window
  {

    private readonly IEnumerable<ITTSProvider> _providers;
    private ApplicationSettings _applicationSettings;

    public ProvidersSettingsWindow(IEnumerable<ITTSProvider> providers, ApplicationSettings applicationSettings)
    {
      InitializeComponent();
      this._providers = providers;
      this._applicationSettings = applicationSettings;

      providerComboBox.ItemsSource = this._providers.ToList();
      providerComboBox.SelectedIndex = 0;

      populateListBox();
    }

    private void populateListBox()
    {
      providersListBox.ItemsSource = _applicationSettings.ConfiguredProviders;
    }

    private void addProviderButton_Click(object sender, RoutedEventArgs e)
    {

      var provider = (ITTSProvider)providerComboBox.SelectedItem;
      var count = _applicationSettings.ConfiguredProviders.Count + 1;

      _applicationSettings.AddConfiguredProvider(new Settings.ConfiguredProvider()
      {
        Name = provider.Name + " " + count,
        ProviderName = provider.GetType().ToString()
      });

      populateListBox();
      _applicationSettings.Save();
    }

    private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var listBoxItem = sender as ListBoxItem;
      if (listBoxItem != null && listBoxItem.IsSelected)
      {
        // The item is already selected and has been clicked again
        var item = (ConfiguredProvider)listBoxItem.DataContext;  // DataContext holds the bound item
        item.Provider.OpenSettings();

      }
    }

    private void ListBoxItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
      var listBoxItem = sender as ListBoxItem;
      if (listBoxItem != null && listBoxItem.IsSelected)
      {
        // The item is already selected and has been clicked again
        //var item = (ConfiguredProvider)listBoxItem.DataContext;  // DataContext holds the bound item
        //_applicationSettings.ConfiguredProviders.Remove(item);
        //populateListBox();
        //_applicationSettings.Save();
      }
    }
  }
}
