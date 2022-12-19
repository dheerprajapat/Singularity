using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode.Playlists;
using YoutubeExplode.Search;

namespace Singularity.Models;
public class PlaylistVideoSearchResult : ISearchResult
{
    public string Url => PlaylistVideo.Url;

    public string Title => PlaylistVideo.Title;

    public PlaylistVideo PlaylistVideo
    {
        get;private set;
    }
    public PlaylistVideoSearchResult(PlaylistVideo playlistVideo)
    {
        PlaylistVideo= playlistVideo;
    }
}
