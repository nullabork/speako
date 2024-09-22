using Accord;
using Omu.ValueInjecter;
using speako.Common;
using speako.Services.Auth;
using speako.Services.Providers;
using speako.Settings;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace speako.Services.ProviderSettings
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


      var control = provider.SettingsControl();
      SettingsWindow settingsWindow = new SettingsWindow();
      settingsWindow.settingsScroller.Content = control;


      control.Saved += (sender, e) =>
      {
        _applicationSettings.ConfiguredProviders.Add(e);
        _applicationSettings.Save();
        settingsWindow.Close();
      };

      control.Cancel += (sender, e) =>
      {
        settingsWindow.Close();
      };

      settingsWindow.ShowDialog();

    }

    public void EditProvider(IAuthSettings authSettings)
    {
      var provider = authSettings.Provider;

      var control = provider.SettingsControl();
      SettingsWindow settingsWindow = new SettingsWindow();
      settingsWindow.settingsScroller.Content = control;
      var authIndex = _applicationSettings.ConfiguredProviders.IndexOf(authSettings);

      control.SetAuthSettings(authSettings);
      settingsWindow.Closing += (sender, e) =>
      {
        if(control.SaveOnClosing())
        {
          authSettings.InjectFrom(control.GetAuthSettings());
          _applicationSettings.Save();
        }
      };
      
      control.Saved += (sender, toSave) =>
      {
        authSettings.InjectFrom(control.GetAuthSettings());
        _applicationSettings.Save();
      };

      control.Cancel += (sender, e) =>
      {
        settingsWindow.Close();
      };

      settingsWindow.ShowDialog();
    }


    private void DuplicateSettings(IAuthSettings authSettings)
    {
      var newAuth = authSettings.Duplicate();
      var sIndex = _applicationSettings.ConfiguredProviders.IndexOf(authSettings);
      _applicationSettings.ConfiguredProviders.Insert(sIndex + 1, newAuth);
      _applicationSettings.Save();
    }

    private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var authSettings = (IAuthSettings)providersListBox.SelectedItem;
      EditProvider(authSettings);
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
      var item = (IAuthSettings)providersListBox.SelectedItem;
      if (item != null)
      {
        EditProvider(item);
      }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
      var item = (IAuthSettings)providersListBox.SelectedItem;
      if (item != null)
      {

        var profiles = _applicationSettings.ConfiguredVoices.Where(vp => vp.ConfiguredProviderGUID == item.GUID);
        var pCount = profiles.Count();

        var profileWarning = "";
        if (pCount > 0)
        {
          profileWarning = $", it has {pCount} voice profiles associated with it";
        }

        MessageBoxResult providerResult = MessageBox.Show($"Are you sure you want to delete {item.Name ?? "this"} provider{profileWarning}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        var deleteProvider = providerResult == MessageBoxResult.Yes;
        //Dont delete
        if (!deleteProvider) return;

        _applicationSettings.ConfiguredProviders.Remove(item);
        _applicationSettings.Save();

        //theres no profiles anyway dont continmue
        if (pCount == 0) return;

        MessageBoxResult profileResult = MessageBox.Show($"Do you want to delete the {pCount} voice profile{(pCount > 1 ? "s" : "")} associated with this provider?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        var deleteProfiles = profileResult == MessageBoxResult.Yes;
        //dont delete profiles we can just change the provider
        if (!deleteProfiles) return;

        profiles.ToList().ForEach(p => {
          _applicationSettings.ConfiguredVoices.Remove(p);
        });
        _applicationSettings.Save();
      }
    }

    private void Duplicate_Click(object sender, RoutedEventArgs e)
    {
      var authSettings = (IAuthSettings)providersListBox.SelectedItem;
      if (authSettings != null)
      {
        DuplicateSettings(authSettings);
      }
    }

    private async void Test_Click(object sender, RoutedEventArgs e)
    {

      var item = (IAuthSettings)providersListBox.SelectedItem;
      if (item != null)
      {
        var connected = await item.Provider.CanConnectToTTSClient();
        
        if (connected)
        {
          MessageBox.Show($"Provider {item.Name ?? " "} seems to be configured correctly", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        } else {
          MessageBox.Show($"Provider {item.Name ?? ""} is probably configured incorrectly", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
        }
      }
    }


  }
}
