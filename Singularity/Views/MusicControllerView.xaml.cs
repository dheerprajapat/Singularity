
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Singularity.ViewModels;
using Singularity.Models;
using Singularity.Core.Contracts.Services;

namespace Singularity.Views;

public sealed partial class MusicControllerView : UserControl
{

    /// <summary>
    /// Exposed View Model as Static Object as we only have one instace any way
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static MusicCotrollerViewModel ExViewModel
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        get;private set;
    }
    public MusicCotrollerViewModel ViewModel
    {
        get; private set;
    }


    public MusicControllerView()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<MusicCotrollerViewModel>();
        var UserSettings = App.GetService<IUserSettingsService>().CurrentSetting;

        ExViewModel = ViewModel;
        ViewModel.InitPlayer(videoPlayer);
        volumeSlider.Value = UserSettings.Media.Volume;
        ViewModel.SetVolume((int)volumeSlider.Value);
    }

    ~MusicControllerView()
    {
        videoPlayer?.MediaPlayer?.Dispose();
    }

    private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        if(Math.Abs(e.NewValue-e.OldValue)>=3 && ViewModel is not null)
            ViewModel.PositionChanged((int)e.NewValue);
    }

    private void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        if(ViewModel is not null)
            ViewModel.SetVolume((int)e.NewValue);
    }
}
