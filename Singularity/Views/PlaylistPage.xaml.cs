using Microsoft.UI.Xaml.Controls;

using Singularity.ViewModels;

namespace Singularity.Views;

public sealed partial class PlaylistPage : Page
{
    public PlaylistViewModel ViewModel
    {
        get;
    }

    public PlaylistPage()
    {
        ViewModel = App.GetService<PlaylistViewModel>();
        InitializeComponent();
    }
}
