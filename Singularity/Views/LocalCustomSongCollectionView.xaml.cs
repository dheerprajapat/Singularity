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
using System.Collections.ObjectModel;
using Singularity.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Singularity.Views
{
    public sealed partial class LocalCustomSongCollectionView : UserControl
    {
        public LocalCustomSongCollectionView()
        {
            this.InitializeComponent();
        }
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(LocalCustomSongCollectionView), new PropertyMetadata(""));


        public string Author
        {
            get => (string)GetValue(AuthorProperty);
            set => SetValue(AuthorProperty, value);
        }

        // Using a DependencyProperty as the backing store for Author.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AuthorProperty =
            DependencyProperty.Register("Author", typeof(string), typeof(LocalCustomSongCollectionView), new PropertyMetadata(""));



        public string Thumbnail
        {
            get => (string)GetValue(ThumbnailProperty);
            set => SetValue(ThumbnailProperty, value);
        }

        // Using a DependencyProperty as the backing store for Thumbnail.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbnailProperty =
            DependencyProperty.Register("Thumbnail", typeof(string), typeof(LocalCustomSongCollectionView), new PropertyMetadata(null));

        public ObservableCollection<string> SongListItems
        {
            get => (ObservableCollection<string>)GetValue(SongListItemsProperty);
            set => SetValue(SongListItemsProperty, value);
        }

        public static readonly DependencyProperty SongListItemsProperty =
            DependencyProperty.Register("SongListItems", typeof(ObservableCollection<string>),
                typeof(LocalCustomSongCollectionView), new PropertyMetadata(new ObservableCollection<string>()));



        public SongStringPageInfoModel? MetaInfo
        {
            get => (SongStringPageInfoModel?)GetValue(MetaInfoProperty);
            set => SetValue(MetaInfoProperty, value);
        }

        // Using a DependencyProperty as the backing store for MetaInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MetaInfoProperty =
            DependencyProperty.Register("MetaInfo", typeof(SongStringPageInfoModel), typeof(LocalCustomSongCollectionView), new PropertyMetadata(null));



    }
}
