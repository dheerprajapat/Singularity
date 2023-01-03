using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Singularity.Core.Contracts.Services;
using Singularity.Core.Services;

namespace Singularity.ViewModels;
public partial class LikesViewModel: ObservableRecipient
{
    [ObservableProperty]
    public ObservableCollection<string>? likedSongs;

    public LikesViewModel(IUserSettingsService userSettingsService)
    {
        UserSettingsService = userSettingsService;

        LikedSongs=new(userSettingsService.CurrentSetting.LikedSongs);
        userSettingsService.CurrentSetting.LikedSongs.CollectionChanged += LikedSongs_CollectionChanged;
    }

    ~LikesViewModel()
    {
        UserSettingsService.CurrentSetting.LikedSongs.CollectionChanged -= LikedSongs_CollectionChanged;
    }

    private void LikedSongs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(LikedSongs));
    }

    public IUserSettingsService UserSettingsService
    {
        get;
    }
}
