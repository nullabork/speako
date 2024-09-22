using speako.Common;
using speako.Services.Auth;
using speako.Services.ProviderSettings;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;


namespace speako.Services.Providers.Google
{
  /// <summary>
  /// Interaction logic for GoogleSettingsControl.xaml
  /// </summary>
  public partial class GoogleSettingsControl : UserControl, IProviderSettingsControl
  {
    public event EventHandler<IAuthSettings> Saved;
    public event EventHandler<IAuthSettings> Cancel;

    private IAuthSettings _originalAuthSettings;

    public GoogleSettingsControl()
    {
      InitializeComponent();
      DataContext = new GoogleAuthSettings();
    }

    //set data context
    public void SetAuthSettings(IAuthSettings settings)
    {
      _originalAuthSettings = settings;
      var cloned = ObjectUtils.Clone(settings);
      cloned.PropertyChanged += OnPropertyChanged;
      this.DataContext = cloned;
      SaveButtonState();
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      SaveButtonState();
    }

    private void SaveButtonState()
    {
      var isEqual = Compare.AreObjectsEqual((GoogleAuthSettings)this.DataContext, (GoogleAuthSettings)_originalAuthSettings);
      saveButton.IsEnabled = !isEqual;
    }

    public IAuthSettings GetAuthSettings()
    {
      return (IAuthSettings)DataContext;
    }

    private void input_TextChanged(object sender, TextChangedEventArgs e)
    {
      
    }

    public bool SaveOnClosing()
    {
      if (!Compare.AreObjectsEqual((GoogleAuthSettings)this.DataContext, (GoogleAuthSettings)_originalAuthSettings))
      {
        MessageBoxResult result = MessageBox.Show($"Do you want to save your changes?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        return result == MessageBoxResult.Yes;
      }
      return false;
    }

    private void saveButton_Click(object sender, RoutedEventArgs e)
    {
      var settings = (IAuthSettings)this.DataContext;
      Saved.Invoke(this, settings);
      SaveButtonState();
    }

    private void cancelButton_Click(object sender, RoutedEventArgs e)
    {
      var settings = (IAuthSettings)this.DataContext;
      Cancel.Invoke(this, settings);
    }
  }
}
