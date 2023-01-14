using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Singularity.Contracts.Services;
using Singularity.Core.Contracts.Services;
using Singularity.Helpers;
using Singularity.Models;
using YoutubeExplode.Videos;

namespace Singularity.ViewModels;
public partial class VideoIdListViewModel : ObservableRecipient, ICrossThreadOperable
{
    [ObservableProperty]
    public ObservableCollection<string>? videoIds;
    public static IYoutubeService Youtube
    {
        get; private set;
    }
    public INavigationService NavigationService
    {
        get;
    }
    public IUserSettingsService UserSettingsService
    {
        get;
    }
    public SongStringPageInfoModel? MetaInfo
    {
        get;set;
    }

    public VideoIdListViewModel(IYoutubeService youtube,INavigationService navigationService,
        IUserSettingsService userSettingsService)
    {
        Youtube = youtube;
        NavigationService = navigationService;
        UserSettingsService = userSettingsService;
    }

    [AlsoNotifyChangeFor(nameof(ShowProgress))]
    [AlsoNotifyChangeFor(nameof(ShowItems))]
    [ObservableProperty]
    public ObservableCollection<SearchFragmentItem>? songs;

    public Visibility ShowProgress => Songs==null ? Visibility.Visible:Visibility.Collapsed;
    public Visibility ShowItems => Songs==null ? Visibility.Collapsed : Visibility.Visible;

    public DispatcherQueue DispatcherQueue { get; set; } = DispatcherQueue.GetForCurrentThread();

    ~VideoIdListViewModel()
    {
        UserSettingsService.CurrentSetting.OnLikePageToggledForId -= CurrentSetting_OnLikePageToggledForId;
        AudioQueue.OnCurrentVideoIdChanged -= AudioQueue_OnCurrentVideoIdChanged;

    }

    public async void InitSongItems(ObservableCollection<string> videoIds)
    {
        VideoIds = videoIds;

        if (Songs != null)
            return;
        Songs = new ObservableCollection<SearchFragmentItem>();

        if (videoIds != null)
        {
            HandleEventsRelatedToPage();

            foreach (var vidId in VideoIds.Distinct())
            {
                var song = await GetFragmentFromId(vidId);
                Songs.Add(song);
                song.MetaInfo = MetaInfo;
            }
        }

    }
    public static async Task<SearchFragmentItem> GetFragmentFromId(string id)
    {
        var video = await Youtube.GetVideoInfo(id);

        return new SearchFragmentItem()
        {
            ChannelName = video.Author.ChannelTitle,
            Duration = MediaPlayerHelper.ConvertTimeSpanToDuration(video.Duration.GetValueOrDefault()),
            MediaType = "Video",
            Name = video.Title,
            ThumbnailUrl = video.Thumbnails.GetBestThumbnail(),
            Item = video
        };
    }
    private void HandleEventsRelatedToPage()
    {
        var currentPage = NavigationService.GetCurrentPageType();

        if (currentPage == Services.CurrentPageType.Like)
        {
            UserSettingsService.CurrentSetting.OnLikePageToggledForId += CurrentSetting_OnLikePageToggledForId;
        }
        else if (currentPage == Services.CurrentPageType.RecentPlays)
        {
            AudioQueue.OnCurrentVideoIdChanged += AudioQueue_OnCurrentVideoIdChanged;
        }
    }

    private void AudioQueue_OnCurrentVideoIdChanged(string? id)
    {
        if (id == null)
            return;

        var item = Songs?.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            var index = Songs?.IndexOf(item);
            Songs?.Move(index.GetValueOrDefault(), 0);
        }
    }

    private async void CurrentSetting_OnLikePageToggledForId(string id, bool added = false)
    {
        if (!added && Songs!=null)
        {
            VideoIds?.Remove(id);
            var item = Songs.FirstOrDefault(x => x.Id == id);
            if (item != null)
                Songs.Remove(item);
        }
        else if(Songs!=null)
        {
            var song = await GetFragmentFromId(id);
            song.MetaInfo = MetaInfo;
            Songs.Insert(0,song);
        }
    }
}
