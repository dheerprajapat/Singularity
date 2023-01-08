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

    private async void NewBtn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var cd = new ContentDialog()
        {
            Title = "Add New Playlist",
            Content = "Ok",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
        };
        cd.XamlRoot = this.XamlRoot;


        var playlisTxtBox = new TextBox()
        {
            PlaceholderText = "Playlist Name",
        };
        playlisTxtBox.TextChanged += (sender, e) =>
        {
            cd.PrimaryButtonText = !string.IsNullOrWhiteSpace(playlisTxtBox.Text) ? "Create" : "";
        };
        cd.Content = playlisTxtBox;
        cd.PrimaryButtonClick += (_, _) => ViewModel.CreateNewPlaylist(playlisTxtBox.Text);
        await cd.ShowAsync();
    }
}
