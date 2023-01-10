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
using Singularity.Contracts.Services;
using Singularity.Services;
using Singularity.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Singularity.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SongStringCollectionPage : Page
{
    public SongStringCollectionPage()
    {
        ViewModel = App.GetService<SongStringCollectionPageViewModel>();
        NavigationService = App.GetService<INavigationService>();
        NavigationService.Navigated += NavigationService_Navigated;

        this.InitializeComponent();
    }

    internal SongStringCollectionPageViewModel ViewModel
    {
        get;
    }
    public INavigationService NavigationService
    {
        get;
    }
    private void NavigationService_Navigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        if (e.SourcePageType.FullName == typeof(SongStringCollectionPage).FullName)
        {
            var json = e.Parameter.ToString();
            ViewModel.InitInfo(json);
        }
        else
        {
            NavigationService.Navigated -= NavigationService_Navigated;
        }
    }
}
