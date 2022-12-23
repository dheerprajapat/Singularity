using CommunityToolkit.Mvvm.ComponentModel;
using Singularity.Core.Helpers;
using Singularity.Core.Services;

namespace Singularity.ViewModels;

public partial class HomeViewModel : ObservableRecipient
{

    public HomeViewModel()
    {
        LoadFeedAsync();
    }

    public async void LoadFeedAsync()
    {
    }
}
