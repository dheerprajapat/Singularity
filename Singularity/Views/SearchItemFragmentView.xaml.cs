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
using Singularity.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using YoutubeExplode.Search;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Singularity.Views;
public sealed partial class SearchItemFragmentView : UserControl
{
    public SearchItemFragmentViewModel ViewModel
    {
        get;
    }


    public IAsyncEnumerable<ISearchResult> Items
    {
        get=>(IAsyncEnumerable<ISearchResult>)GetValue(ItemsProperty);
        set
        {
            SetValue(ItemsProperty, value);
            ViewModel.Items = value;
            _ = ViewModel.ProcessSerchItems();
        }
    }

    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.Register("Items", typeof(IAsyncEnumerable<ISearchResult>),
            typeof(SearchItemFragmentView),
            new PropertyMetadata(null));



    public string? Header
    {
        get => (string)GetValue(HeaderProperty);
        set
        {
            SetValue(HeaderProperty, value);
            ViewModel.Header = value;
        }
    }

    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register("Header", typeof(string), typeof(SearchItemFragmentView),
            new PropertyMetadata(null));



    public bool ShowAllItems
    {
        get
        {
            return (bool)GetValue(ShowAllItemsProperty);
        }
        set
        {
           ViewModel.ShowAllItems= value;
            SetValue(ShowAllItemsProperty, value);
        }
    }

    // Using a DependencyProperty as the backing store for ShowAllItems.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ShowAllItemsProperty =
        DependencyProperty.Register("ShowAllItems", typeof(bool), typeof(SearchItemFragmentView), new PropertyMetadata(0));




    public SearchItemFragmentView()
    {
        ViewModel = App.GetService<SearchItemFragmentViewModel>();
        this.InitializeComponent();
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e)
    {
        ViewModel.SelectionChanged((sender as ListView)!.SelectedIndex);
    }
}
