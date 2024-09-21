using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace speako.Services.VoiceSettings
{
  /// <summary>
  /// Interaction logic for VoiceProfileSpeakControl.xaml
  /// </summary>
  public partial class VoiceProfileSpeakControl : UserControl
  {
    public VoiceProfileSpeakControl()
    {
      InitializeComponent();
      this.DataContextChanged += VoiceContextChanged;
    }

    private void VoiceContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      


    }

    public VoiceProfile VoiceContext
    {
      set
      {
        DataContext = value;
        value.PropertyChanged += OnPropertyChanged;
      }
      get
      {

        if (DataContext == null)
        {
          DataContext = new VoiceProfile();
        }

        return (VoiceProfile)DataContext;
      }
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      //throw new NotImplementedException();
    }
  }
}
