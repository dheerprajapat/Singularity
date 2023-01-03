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
using Singularity.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Singularity.Views;
public sealed partial class BannerView : UserControl
{
    public BannerView()
    {
        ViewModel = App.GetService<BannerViewModel>();
        this.InitializeComponent();
    }

    internal BannerViewModel ViewModel
    {
        get;
    }


    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(BannerView), new PropertyMetadata(""));


    public string Author
    {
        get => (string)GetValue(AuthorProperty);
        set => SetValue(AuthorProperty, value);
    }

    // Using a DependencyProperty as the backing store for Author.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AuthorProperty =
        DependencyProperty.Register("Author", typeof(string), typeof(BannerView), new PropertyMetadata(""));



    public ImageSource Thumbnail
    {
        get => (ImageSource)GetValue(ThumbnailProperty);
        set => SetValue(ThumbnailProperty, value);
    }

    // Using a DependencyProperty as the backing store for Thumbnail.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ThumbnailProperty =
        DependencyProperty.Register("Thumbnail", typeof(ImageSource), typeof(BannerView), new PropertyMetadata(null));



    public int BlurAmount
    {
        get => (int)GetValue(BlurAmountProperty);
        set => SetValue(BlurAmountProperty, value);
    }

    // Using a DependencyProperty as the backing store for BlurAmount.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty BlurAmountProperty =
        DependencyProperty.Register("BlurAmount", typeof(int), typeof(BannerView), new PropertyMetadata(0));


}
