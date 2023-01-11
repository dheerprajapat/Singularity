using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Singularity.Models;

namespace Singularity.ViewModels;
public partial class RecentPlayViewModel : ObservableRecipient
{
    [ObservableProperty]
    public ObservableCollection<string>? recentSongs;

    public RecentPlayViewModel()
    {
        RecentSongs = AudioQueue.currentVideoIds;
    }
}
