// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Singularity.Models;
using Singularity.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Singularity.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class VideoIdListView : Page
{
    public VideoIdListView()
    {
        ViewModel= App.GetService<VideoIdListViewModel>();
        this.InitializeComponent();
    }

    public VideoIdListViewModel ViewModel
    {
        get;
    }


    public ObservableCollection<string> SongListItems
    {
        get => (ObservableCollection<string>)GetValue(SongListItemsProperty);
        set
        {
            SetValue(SongListItemsProperty, value);
            ViewModel.InitSongItems(value);
        }
    }

    public static readonly DependencyProperty SongListItemsProperty =
        DependencyProperty.Register("SongListItems", typeof(ObservableCollection<string>), typeof(VideoIdListView), new PropertyMetadata(new ObservableCollection<string>()));

    private void ListView_ItemClick(object sender, ItemClickEventArgs e)
    {
        (e.ClickedItem as SearchFragmentItem)!.DoAction();
    }
}
