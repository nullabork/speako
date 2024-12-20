﻿using Amazon;
using Microsoft.Extensions.DependencyInjection;
using Omu.ValueInjecter;
using speako.Services.Auth;
using speako.Services.ProviderSettings;
using speako.Services.Speak;
using speako.Settings;
using System;
using System.Windows;
using System.Windows.Controls;

namespace speako.Services.VoiceProfiles
{
  /// <summary>
  /// Interaction logic for VoiceSettingsWindow.xaml
  /// </summary>
  public partial class VoiceProfilesListWindow : Window
  {
    private readonly IServiceProvider _serviceProvider;
    private readonly ApplicationSettings _applicationSettings;
    private readonly SpeakService _speakService;

    public VoiceProfilesListWindow(IServiceProvider serviceProvider, ApplicationSettings applicationSettings, SpeakService speak)
    {
      InitializeComponent();
      _serviceProvider = serviceProvider;
      _applicationSettings = applicationSettings;
      _speakService = speak;
      voicesListBox.ItemsSource = _applicationSettings.ConfiguredVoices;
    }

    private async void AddVoiceButton_Click(object sender, RoutedEventArgs ev)
    {
      var window = _serviceProvider.GetRequiredService<VoiceProfileDetailWindow>();
      var voice = new VoiceProfile();
      window.ConfigureVoice(voice);
      window.Saved += (sender, e) =>
      {
        _applicationSettings.ConfiguredVoices.Add(e);
        _applicationSettings.Save();
      };
      window.ShowDialog();
    }

    private async void EditProfile(VoiceProfile profile)
    {
      var window = _serviceProvider.GetRequiredService<VoiceProfileDetailWindow>();
      //load the profile into the window
      window.ConfigureVoice(profile);
      //kep track of profiles index
      var pIndex = _applicationSettings.ConfiguredVoices.IndexOf(profile);
      //wait for window to save
      window.Saved += (sender, e) =>
      {
        //is a copy of the profile with new values inject them into the old profile
        profile.InjectFrom(e);

        //persist the changes
        _applicationSettings.Save();
      };
      //show the window
      window.ShowDialog();
    }

    private void DuplicateProfile(VoiceProfile profile)
    {
      var newProfile = new VoiceProfile();
      newProfile.InjectFrom(profile);
      newProfile.GUID = Guid.NewGuid().ToString();
      newProfile.Name = $"Copy - {profile.Name}";

      var pIndex = _applicationSettings.ConfiguredVoices.IndexOf(profile);
      _applicationSettings.ConfiguredVoices.Insert(pIndex + 1, newProfile);
      _applicationSettings.Save();
    }

    private async void ListBoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      var listBoxItem = sender as ListBoxItem;
      if (listBoxItem != null && listBoxItem.IsSelected)
      {
        var item = (VoiceProfile)listBoxItem.DataContext;
        EditProfile(item);
      }
    }


    private void voicesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private async void Test_Click(object sender, RoutedEventArgs e)
    {
      //var settings = (IAuthSettings)providerComboBox.SelectedItem;
      var item = (VoiceProfile)voicesListBox.SelectedItem;



      await _speakService.SpeakText(item.TTSTestSentence, item);
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
      var item = (VoiceProfile)voicesListBox.SelectedItem;
      if (item != null)
      {
        EditProfile(item);
      }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
      var item = (VoiceProfile)voicesListBox.SelectedItem;
      if (item != null)
      {
        MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {item.Name ?? "this item"}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
          _applicationSettings.ConfiguredVoices.Remove(item);
          _applicationSettings.Save();
        }
      }
    }

    private void Duplicate_Click(object sender, RoutedEventArgs e)
    {
      var item = (VoiceProfile)voicesListBox.SelectedItem;
      if (item != null)
      {
        DuplicateProfile(item);
      }
    }
  }
}
