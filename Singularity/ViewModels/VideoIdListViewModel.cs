using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Singularity.Core.Contracts.Services;
using Singularity.Helpers;
using Singularity.Models;

namespace Singularity.ViewModels;
public partial class VideoIdListViewModel : ObservableRecipient
{
    public ObservableCollection<string>? VideoIds
    {
        get; set;
    }
    public IYoutubeService Youtube
    {
        get;
    }

    public VideoIdListViewModel(IYoutubeService youtube)
    {
        Youtube = youtube;
    }

    [AlsoNotifyChangeFor(nameof(ShowProgress))]
    [AlsoNotifyChangeFor(nameof(ShowItems))]
    [ObservableProperty]
    public ObservableCollection<SearchFragmentItem>? songs;

    public Visibility ShowProgress => Songs==null ? Visibility.Visible:Visibility.Collapsed;
    public Visibility ShowItems => Songs==null ? Visibility.Collapsed : Visibility.Visible;

    public async void InitSongItems(ObservableCollection<string> videoIds)
    {
        VideoIds = videoIds;
        Songs = new ObservableCollection<SearchFragmentItem>();
        if(videoIds!=null)
        foreach (var vidId in VideoIds)
        {
            var video = await Youtube.GetVideoInfo(vidId);
            Songs.Add(new SearchFragmentItem()
            {
                ChannelName = video.Author.ChannelTitle,
                Duration = MediaPlayerHelper.ConvertTimeSpanToDuration(video.Duration.GetValueOrDefault()),
                MediaType = "Video",
                Name = video.Title,
                ThumbnailUrl = video.Thumbnails.GetBestThumbnail(),
                Item = video
            }); ;
        }

    }
}
