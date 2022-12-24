using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Singularity.Core.Contracts.Services;
using Singularity.Models;
using YoutubeExplode.Playlists;
using YoutubeExplode.Search;

namespace Singularity.ViewModels;
public partial class HomeQuickPlayViewModel:ObservableRecipient
{

    public readonly PlaylistId Id = PlaylistId.Parse("PLOHoVaTp8R7d3L_pjuwIa6nRh4tH5nI4x");

    public ImageSource Thumbnail = new BitmapImage
    {
        UriSource = new Uri("https://c4.wallpaperflare.com/wallpaper/733/547/686/abstract-spectrum-music-bardi-wallpaper-preview.jpg")
    };

    public IYoutubeService Youtube
    {
        get;
    }
    public HomeQuickPlayViewModel(IYoutubeService youtube)
    {
        Youtube = youtube;

    }
}
