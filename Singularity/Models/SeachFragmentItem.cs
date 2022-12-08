using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using YoutubeExplode.Search;

namespace Singularity.Models;
public class SearchFragmentItem
{
    public string? Name { get; set; }
    public string? MediaType { get; set; }
    public string? ChannelName { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Duration
    {
        get; set;
    }
    public Visibility HideDuration =>  MediaType!="Video"
    ? Visibility.Collapsed : Visibility.Visible;

}
