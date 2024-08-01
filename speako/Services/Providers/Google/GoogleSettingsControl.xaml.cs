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
        public GoogleSettingsControl()
        {
            InitializeComponent();
            this.DataContext = GoogleAuthSettings.Instance;
        }

        private void input_TextChanged(object sender, TextChangedEventArgs e)
        {
            GoogleAuthSettings.Instance.Save();
        }
    }
}
