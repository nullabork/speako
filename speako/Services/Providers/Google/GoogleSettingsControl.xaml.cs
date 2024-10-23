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

    private GoogleAuthSettings _original;
    private GoogleAuthSettings _working;

    public GoogleSettingsControl()
    {
      InitializeComponent();
      DataContext = new GoogleAuthSettings();
    }

    //set data context
    public void Configure(IAuthSettings settings)
    {

      var og = (GoogleAuthSettings)settings;
      var current = ObjectUtils.Clone(og);

      _original = og;
      _working = current;

      _working.PropertyChanged += OnPropertyChanged;
      this.DataContext = _working;
      SaveButtonState();
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      SaveButtonState();
    }

    private void SaveButtonState()
    {
      var isEqual = Compare.ObjectsPropertiesEqual(_working, _original);
      saveButton.IsEnabled = !isEqual;
    }

    private void input_TextChanged(object sender, TextChangedEventArgs e)
    {
      
    }

    public IAuthSettings SaveOnClosing()
    {
      if (!Compare.ObjectsPropertiesEqual(_working, _original))
      {
        MessageBoxResult result = MessageBox.Show($"Do you want to save your changes?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if(result == MessageBoxResult.Yes)
        {
          return _working;
        }
      }
      return null;
    }

    private void saveButton_Click(object sender, RoutedEventArgs e)
    {
      
      Saved.Invoke(this, _working);
      Configure(_working);
    }

    private void cancelButton_Click(object sender, RoutedEventArgs e)
    {
      Cancel.Invoke(this, _working);
    }
  }
}
