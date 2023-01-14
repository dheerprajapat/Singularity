using System.Collections.ObjectModel;
using System.Text.Json;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Singularity.Contracts.Services;
using Singularity.Core.Contracts.Services;
using Singularity.Core.Models;
using Singularity.Core.Services;
using Singularity.Models;
using Singularity.ViewModels;

namespace Singularity.Views;

public sealed partial class PlaylistPage : Page
{
    public PlaylistViewModel ViewModel
    {
        get;
    }
    public INavigationService NavService
    {
        get;
    }

    public PlaylistPage()
    {
        ViewModel = App.GetService<PlaylistViewModel>();
        NavService = App.GetService<INavigationService>();
        InitializeComponent();
    }

    private async void NewBtn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

        await CreatePlaylistDialog(XamlRoot);
    }

    public static async Task CreatePlaylistDialog(XamlRoot xamlRoot,string? song=null)
    {
        var cd = new ContentDialog()
        {
            Title = "Add New Playlist",
            Content = "Ok",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
        };
        cd.XamlRoot = xamlRoot;


        var playlisTxtBox = new TextBox()
        {
            PlaceholderText = "Playlist Name",
        };
        playlisTxtBox.TextChanged += (sender, e) =>
        {
            cd.PrimaryButtonText = !string.IsNullOrWhiteSpace(playlisTxtBox.Text) ? "Create" : "";
        };
        cd.Content = playlisTxtBox;
        var songs = new ObservableCollection<string>();
        if (song != null)
            songs.Add(song);
        cd.PrimaryButtonClick += (_, _) => App.GetService<IUserSettingsService>()
                .CurrentSetting.PlaylistCollection.Playlists.Add(new(playlisTxtBox.Text,"",songs));

        await cd.ShowAsync();
    }

    private void PlaylistBtn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var dataSource = (sender as Button)!.DataContext as PlaylistItem;
        NavService.NavigateTo(typeof(SongStringCollectionPageViewModel).FullName!,
            JsonSerializer.Serialize(new SongStringPageInfoModel()
            {
                Author = dataSource.Author,
                Items = dataSource.Songs,
                Title = dataSource.Name,
                Thumbnail = dataSource.ThumbnailUrl
            }));
    }
}
