using CommunityToolkit.Mvvm.ComponentModel;
using Singularity.Core.Helpers;
using Singularity.Core.Models.YoutubeJS;
using Singularity.Core.Services;

namespace Singularity.ViewModels;

public partial class HomeViewModel : ObservableRecipient
{
    [ObservableProperty]
    public MusicFeed? feed;

    public HomeViewModel()
    {
    }

    private void DenoServerLauncher_DenoServerStarted()
    {
        LoadFeedAsync();
    }

    public async void LoadFeedAsync()
    {
    }
}
