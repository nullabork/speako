using speako.Common;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace speako.Services.PostProcessors.Discord
{
  /// <summary>
  /// Interaction logic for DiscordSettingsControl.xaml
  /// </summary>
  public partial class DiscordSettingsControl : UserControl, IPostProcessorControl
  {

    private IPostProcessorSettings _original;
    private IPostProcessorSettings _working;
    public DiscordSettingsControl()
    {
      InitializeComponent();
    }

    public event EventHandler<IPostProcessorSettings> Saved;
    public event EventHandler<IPostProcessorSettings> Cancel;

    public async Task Configure(IPostProcessorSettings settings)
    {

      var og = (DiscordProcessorSettings)settings;
      var current = (DiscordProcessorSettings)ObjectUtils.Clone(settings);
      current.ChannelIds = new ObservableCollection<DiscordChannel>(og.ChannelIds);

      _original = og;
      _working = current;

      _working.PropertyChanged += OnPropertyChanged;
      this.DataContext = _working;
      SaveButtonState();
      return;
    }

    private async Task LoadDiscordGuilds()
    {
      var pp = (DiscordProcessor) _working.GetPostProcessor();
      await pp.Configure(_working);
      var guilds = await pp.GetDiscordChannels();

      if (guilds != null && guilds.Count > 0)
      {
        selectChannels.IsEnabled = true;
      } else
      {
        selectChannels.IsEnabled = false;
      }

      guildComboBox.ItemsSource = guilds;
    }


    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      SaveButtonState();
    }

    private void SaveButtonState()
    {
      var isEqual = Compare.ObjectsPropertiesEqual((DiscordProcessorSettings)_working, (DiscordProcessorSettings)_original);
      saveButton.IsEnabled = !isEqual && !string.IsNullOrEmpty(_working.Name);
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

    private void addChannelButton_click(object sender, RoutedEventArgs e)
    {
      var channel = (DiscordChannel)channelsComboBox.SelectedItem;
      var current = (DiscordProcessorSettings)_working;
      current.ChannelIds.Add(channel);
      SaveButtonState();
    }

    private void delChannelButton_click(object sender, RoutedEventArgs e)
    {
      Delete_Click(sender, e);
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
      var channel = (DiscordChannel)selectedChannels.SelectedItem;
      var current = (DiscordProcessorSettings)_working;
      current.ChannelIds.Remove(channel);
      SaveButtonState();
    }

    private async void botToken_TextChanged(object sender, TextChangedEventArgs e)
    {
      await LoadDiscordGuilds();
    }

    private void guildComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

      if(guildComboBox.ItemsSource == null)
      {
        return;
      }
      
      var selected = guildComboBox.SelectedItem;
      

      var guild = (DiscordGuild)selected;
      channelsComboBox.ItemsSource = guild.discordChannels;
    }

  }
}
