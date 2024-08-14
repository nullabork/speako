using System;
using System.Collections.Generic;
using System.Windows.Controls;


namespace speako.Services.Providers.Google
{
  /// <summary>
  /// Interaction logic for GoogleSettingsControl.xaml
  /// </summary>
  public partial class GoogleSettingsControl : UserControl
  {

    private GoogleAuthSettings _settings;

    public GoogleSettingsControl()
    {
      InitializeComponent();
    }

    //set data context
    public void SetDataContext(GoogleAuthSettings settings)
    {
      _settings = settings;
      this.DataContext = settings;
    }

    private void input_TextChanged(object sender, TextChangedEventArgs e)
    {
      
    }

    private void button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      _settings.Save();
    }
  }
}
