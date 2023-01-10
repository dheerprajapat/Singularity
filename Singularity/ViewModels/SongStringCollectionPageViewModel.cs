using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Singularity.Models;
using YoutubeExplode.Videos;

namespace Singularity.ViewModels;
internal partial class SongStringCollectionPageViewModel:ObservableRecipient
{
    [ObservableProperty]
    public string? title;

    [ObservableProperty]
    public string? author;

    [ObservableProperty]
    public string? thumbnail;

    [ObservableProperty]
    public ObservableCollection<string>? items;

    internal void InitInfo(string? json)
    {
        var obj = JsonSerializer.Deserialize<SongStringPageInfoModel>(json);
        Title = obj!.Title;
        Author = obj!.Author;
        Items = obj.Items;
        Thumbnail = obj.Thumbnail;
    }
}

