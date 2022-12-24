using Microsoft.UI.Xaml.Controls;

using Singularity.ViewModels;

namespace Singularity.Views;

public sealed partial class HomePage : Page
{
    public HomeViewModel ViewModel
    {
        get;
    }

    public HomePage()
    {
        ViewModel = App.GetService<HomeViewModel>();
        InitializeComponent();
    }


    private void QuickPlay_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        QuickPlay.NavigateToQuickPlaylist();
    }
}
