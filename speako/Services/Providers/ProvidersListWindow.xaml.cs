using Accord;
using Omu.ValueInjecter;
using speako.Common;
using speako.Services.Auth;
using speako.Services.Providers;
using speako.Settings;

using System.Windows;
using System.Windows.Input;

namespace speako.Services.ProviderSettings
{
  public partial class ProvidersListWindow : Window
  {

    private readonly IEnumerable<IAuthSettings> _providerSettings;
    private ApplicationSettings _applicationSettings;

    public ProvidersListWindow(IEnumerable<IAuthSettings> providerSettings, ApplicationSettings applicationSettings)
    {
      InitializeComponent();
      this._providerSettings = providerSettings;
      this._applicationSettings = applicationSettings;

      providerComboBox.ItemsSource = this._providerSettings.ToList();
      providerComboBox.SelectedIndex = 0;

      populateListBox();
    }

    private void populateListBox()
    {
      providersListBox.ItemsSource = _applicationSettings.ProviderSettings;
    }


    //TODO: refactor this to be like like Discord processor - Store and save the settings and have the settings own the provider 
    private void addProviderButton_Click(object sender, RoutedEventArgs e)
    {
      var selected = providerComboBox.SelectedItem;
      var settings = (IAuthSettings)Activator.CreateInstance(selected.GetType());
      EditProvider(settings);

    }

    public void EditProvider(IAuthSettings authSettings)
    {
     

      var control = authSettings.SettingsControl();
      control.Configure(authSettings);

      ProviderDetailWindow settingsWindow = new ProviderDetailWindow();
      settingsWindow.settingsScroller.Content = control;

      var authIndex = _applicationSettings.ProviderSettings.IndexOf(authSettings);


      
      settingsWindow.Closing += (sender, e) =>
      {
        var toSave = control.SaveOnClosing();
        if (toSave != null)
        {
          UpdateOrAdd(toSave);
          _applicationSettings.Save();
        }
      };
      
      control.Saved += (sender, e) =>
      {
        UpdateOrAdd(e);
        _applicationSettings.Save();
      };

      control.Cancel += (sender, e) =>
      {
        settingsWindow.Close();
      };

      settingsWindow.ShowDialog();
    }

    private void UpdateOrAdd(IAuthSettings e)
    {
      var find = _applicationSettings.ProviderSettings.FirstOrDefault((IAuthSettings settings) => settings.GUID == e.GUID, null);
      var index = _applicationSettings.ProviderSettings.IndexOf(find);

      if (find != null && index != -1)
      {
        find.InjectFrom(e);
      }
      else
      {
        _applicationSettings.ProviderSettings.Add(e);
      }
    }


    private void DuplicateSettings(IAuthSettings authSettings)
    {
      var newAuth = ObjectUtils.Clone(authSettings);
      newAuth.Name = $"{newAuth.Name} - copy";
      var sIndex = _applicationSettings.ProviderSettings.IndexOf(authSettings);
      _applicationSettings.ProviderSettings.Insert(sIndex + 1, newAuth);
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

        _applicationSettings.ProviderSettings.Remove(item);
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
        var connected = await item.GetProvider().CanConnectToTTSClient();
        
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
