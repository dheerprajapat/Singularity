﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Singularity.Contracts;
using Singularity.Managers;
using Singularity.Services;
using YoutubeExplode.Videos.Streams;

namespace Singularity.Models;

public class YouTubeSong : ISong
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Singer { get; set; }
    public required string Description { get; set; }
    public required string ThumbnailUrl { get; set; }
    public required TimeSpan? Duration { get; set; }

    public ValueTask<StreamUrl?> GetAudioUrlAsync()
    {
        return SystemManager.GetService<IMusicHub>()!.GetSongStreamUrlAsync(Id);
    }

    public bool Equals(ISong? other)
    {
        return other!=null && Id.Equals(other.Id);
    }
}
