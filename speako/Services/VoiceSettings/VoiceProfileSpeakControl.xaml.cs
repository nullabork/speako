using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

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
