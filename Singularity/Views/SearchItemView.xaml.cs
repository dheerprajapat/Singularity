// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Singularity.Core.Contracts.Services;
using Singularity.Models;
using Singularity.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Singularity.Views;
public sealed partial class SearchItemView : UserControl
{
    public SearchItemViewModel ViewModel
    {
        get;
    }
    public IUserSettingsService UserSettingService
    {
        get;
    }

    public SearchFragmentItem Item
    {
        get => (SearchFragmentItem)GetValue(ItemProperty);
        set
        {
            SetValue(ItemProperty, value);
            ViewModel.Item = value;
            ViewModel.UpdateCurrentPlayingState();
        }
    }

    // Using a DependencyProperty as the backing store for Item.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ItemProperty =
        DependencyProperty.Register("Item", typeof(SearchFragmentItem), typeof(SearchItemView), new PropertyMetadata(null));




    public SearchItemView()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<SearchItemViewModel>();
        UserSettingService = App.GetService<IUserSettingsService>();


    }

    private async void PlaylistBtn_Loading(FrameworkElement sender, object args)
    {
        var m = (sender as MenuFlyoutSubItem)!;
        m.Items.Clear();
        var createNew = new MenuFlyoutItem { Text = "Create New",Icon = new SymbolIcon(Symbol.Add) };
        m.Items.Add(createNew);
        createNew.Click += async(s,e) => await PlaylistPage.CreatePlaylistDialog(this.XamlRoot,ViewModel.Item!.Id);
        foreach (var playlist in UserSettingService.CurrentSetting.PlaylistCollection.Playlists)
        {
            var playlistBtn = new MenuFlyoutItem() { Text = playlist.Name };
            m.Items.Add(playlistBtn);
            playlistBtn.Click += (s, e) =>UserSettingService.CurrentSetting.PlaylistCollection.AddSong(playlist.Name,ViewModel.Item.Id);
        }
    }

}
