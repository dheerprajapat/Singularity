using Microsoft.UI.Xaml.Controls;

using Singularity.WinApp.ViewModels;

namespace Singularity.WinApp.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
