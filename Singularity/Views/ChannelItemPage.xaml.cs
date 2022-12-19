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
using Singularity.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Singularity.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ChannelItemPage : Page
{
    public ChannelItemPage()
    {
        ViewModel = App.GetService<ChannelItemPageViewModel>();
        NavService = App.GetService<INavigationService>();
        NavService.Navigated += NavService_Navigated;
        this.InitializeComponent();
    }


    public ChannelItemPageViewModel ViewModel
    {
        get;
    }
    public INavigationService NavService
    {
        get;
    }
    private void NavService_Navigated(object sender, NavigationEventArgs e)
    {
        if (e.SourcePageType.FullName == typeof(ChannelItemPage).FullName)
        {
            ViewModel.ChannelId = e.Parameter.ToString();
            ViewModel.LoadChannelInfo();
        }
        else
        {
            NavService.Navigated -= NavService_Navigated;
        }
    }
}
