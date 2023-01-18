using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DiscordRPC;
using Singularity.Helpers;
using YoutubeExplode;
using YoutubeExplode.Videos;

namespace Singularity.Contracts.Services;
public class DiscordPresenceService
{
    public DiscordRpcClient client { get; } = new("1065303334929580073");

    public void Initialize()
    {
        client.Initialize();
    }

    public void SetSongInfo(Video video)
    {
        client.SetPresence(new()
        {
            Details =video.Title+" " + video.Author,
            State = "Vibing",
            Buttons = new Button[]
            {
                new Button()
                {
                   Label = "Listen on Singularity",
                   Url = "https://github.com/sps014/Singularity/releases"
                }
            },
            Assets = new Assets()
            {
                LargeImageKey = video.Thumbnails.GetBestThumbnail()
            }
        });
    }

}
