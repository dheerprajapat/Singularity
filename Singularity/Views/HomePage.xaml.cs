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

    private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var index = (sender as GridView)!.SelectedIndex;
        if (index < 0)
            return;

        var id = ViewModel.Genres![index]!.PlaylistId;

        App.GetService<INavigationService>()
           .NavigateTo(typeof(PlaylistItemPageViewModel).FullName!, id);
    }
}
