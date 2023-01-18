using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DiscordRPC;
using Singularity.Helpers;
using Singularity.Models;
using YoutubeExplode;
using YoutubeExplode.Videos;

namespace Singularity.Contracts.Services;
public class DiscordPresenceService
{
    public DiscordRpcClient client { get; } = new("1065303334929580073");

    private static Stopwatch? watch;
    static Stopwatch Watch => watch ??= new Stopwatch();
    const int MaxLimit = 16_000; //16 sec is hard limit

    public void Initialize()
    {
        client.Initialize();
        Watch.Start();
    }


    public void SetSongInfo(Video video,TimeSpan start,TimeSpan end)
    {
        if (start.TotalSeconds>5 && Watch.ElapsedMilliseconds < MaxLimit)
            return;

        Watch.Restart();

        var progress = "Live ⬤";
        var detail = video.Title + " " + video.Author;
        if (end!=TimeSpan.Zero)
        {
            progress = MediaPlayerHelper.ConvertTimeSpanToDuration(start) + " " +
                AsciiProgressBar.GetProgressAscii(start.TotalMilliseconds, end.TotalMilliseconds,9)+" "
                + MediaPlayerHelper.ConvertTimeSpanToDuration(end);
        }
        client.SetPresence(new()
        {
            Details =detail.Substring(0,Math.Min(60,detail.Length-1)),
            State = progress,
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
