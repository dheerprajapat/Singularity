// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

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

namespace Singularity.Views;

public sealed partial class MusicControllerView : UserControl
{
    public MusicCotrollerViewModel ViewModel
    {
        get;
    }

    public MusicControllerView()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<MusicCotrollerViewModel>();
        ViewModel.InitPlayer(videoPlayer);
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
