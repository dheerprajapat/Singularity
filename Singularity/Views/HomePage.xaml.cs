using Microsoft.UI.Xaml.Controls;
using Singularity.Contracts.Services;
using Singularity.Models;
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

    private void GenreBtn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var id =((sender as Button)!.DataContext as Genre)!.PlaylistId;

        App.GetService<INavigationService>()
           .NavigateTo(typeof(PlaylistItemPageViewModel).FullName!,id);
    }
}
