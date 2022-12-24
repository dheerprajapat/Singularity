using CommunityToolkit.Mvvm.ComponentModel;
using Singularity.Core.Contracts.Services;
using Singularity.Core.Helpers;
using Singularity.Core.Services;

namespace Singularity.ViewModels;

public partial class HomeViewModel : ObservableRecipient
{

    public HomeViewModel(IYoutubeService youtube)
    {
        Youtube = youtube;
    }

    public IYoutubeService Youtube
    {
        get;
    }

}
