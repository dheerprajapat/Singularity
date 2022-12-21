using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Singularity.Core.Models.YoutubeJS;


public class Album
{
    [JsonPropertyName("id")]
    public string Id
    {
        get; set;
    }

    [JsonPropertyName("name")]
    public string Name
    {
        get; set;
    }

    [JsonPropertyName("endpoint")]
    public Endpoint Endpoint
    {
        get; set;
    }
}

public class Artist
{
    [JsonPropertyName("name")]
    public string Name
    {
        get; set;
    }

    [JsonPropertyName("channel_id")]
    public string ChannelId
    {
        get; set;
    }

    [JsonPropertyName("endpoint")]
    public Endpoint Endpoint
    {
        get; set;
    }
}


public class Content
{
    [JsonPropertyName("type")]
    public string Type
    {
        get; set;
    }

    [JsonPropertyName("endpoint")]
    public Endpoint Endpoint
    {
        get; set;
    }

    [JsonPropertyName("item_type")]
    public string ItemType
    {
        get; set;
    }

    [JsonPropertyName("id")]
    public string Id
    {
        get; set;
    }

    [JsonPropertyName("title")]
    public object Title
    {
        get; set;
    }

    [JsonPropertyName("album")]
    public Album Album
    {
        get; set;
    }

    [JsonPropertyName("artists")]
    public List<Artist> Artists { get; } = new List<Artist>();

    [JsonPropertyName("thumbnails")]
    public List<Thumbnail> Thumbnails { get; } = new List<Thumbnail>();

    [JsonPropertyName("badges")]
    public List<object> Badges { get; } = new List<object>();


    [JsonPropertyName("item_count")]
    public object ItemCount
    {
        get; set;
    }

    [JsonPropertyName("thumbnail")]
    public List<Thumbnail> Thumbnail { get; } = new List<Thumbnail>();

}

public class Endpoint
{
    [JsonPropertyName("type")]
    public string Type
    {
        get; set;
    }

    [JsonPropertyName("payload")]
    public Payload Payload
    {
        get; set;
    }

    [JsonPropertyName("metadata")]
    public Metadata Metadata
    {
        get; set;
    }
}

public class Header
{
    [JsonPropertyName("type")]
    public string Type
    {
        get; set;
    }

    [JsonPropertyName("title")]
    public Title Title
    {
        get; set;
    }
}

public class Item
{
    [JsonPropertyName("type")]
    public string Type
    {
        get; set;
    }

    [JsonPropertyName("text")]
    public object Text
    {
        get; set;
    }

    [JsonPropertyName("icon_type")]
    public string IconType
    {
        get; set;
    }

    [JsonPropertyName("endpoint")]
    public Endpoint Endpoint
    {
        get; set;
    }

    [JsonPropertyName("toggled_text")]
    public ToggledText ToggledText
    {
        get; set;
    }

    [JsonPropertyName("toggled_icon_type")]
    public string ToggledIconType
    {
        get; set;
    }
}

public class Metadata
{
    [JsonPropertyName("api_url")]
    public string ApiUrl
    {
        get; set;
    }
}

public class Payload
{
    [JsonPropertyName("browseId")]
    public string BrowseId
    {
        get; set;
    }

    [JsonPropertyName("videoId")]
    public string VideoId
    {
        get; set;
    }

    [JsonPropertyName("playlistId")]
    public string PlaylistId
    {
        get; set;
    }

    [JsonPropertyName("params")]
    public string Params
    {
        get; set;
    }


    [JsonPropertyName("queueTarget")]
    public QueueTarget QueueTarget
    {
        get; set;
    }

    [JsonPropertyName("queueInsertPosition")]
    public string QueueInsertPosition
    {
        get; set;
    }


    [JsonPropertyName("serializedShareEntity")]
    public string SerializedShareEntity
    {
        get; set;
    }

    [JsonPropertyName("sharePanelType")]
    public string SharePanelType
    {
        get; set;
    }

    [JsonPropertyName("status")]
    public string Status
    {
        get; set;
    }

    [JsonPropertyName("target")]
    public Target Target
    {
        get; set;
    }
}

public class QueueTarget
{
    [JsonPropertyName("videoId")]
    public string VideoId
    {
        get; set;
    }

    [JsonPropertyName("playlistId")]
    public string PlaylistId
    {
        get; set;
    }
}

public class MusicFeed
{
    [JsonPropertyName("sections")]
    public List<Section> Sections { get; } = new List<Section>();
}

public class Section
{
    [JsonPropertyName("type")]
    public string Type
    {
        get; set;
    }

    [JsonPropertyName("header")]
    public Header Header
    {
        get; set;
    }

    [JsonPropertyName("contents")]
    public List<Content> Contents { get; } = new List<Content>();

    [JsonPropertyName("num_items_per_column")]
    public int? NumItemsPerColumn
    {
        get; set;
    }
}


public class Target
{
    [JsonPropertyName("playlistId")]
    public string PlaylistId
    {
        get; set;
    }

    [JsonPropertyName("video_id")]
    public string VideoId
    {
        get; set;
    }
}


public class Thumbnail
{
    [JsonPropertyName("url")]
    public string Url
    {
        get; set;
    }

    [JsonPropertyName("width")]
    public int? Width
    {
        get; set;
    }

    [JsonPropertyName("height")]
    public int? Height
    {
        get; set;
    }
}

public class Title
{

    [JsonPropertyName("text")]
    public string Text
    {
        get; set;
    }
}

public class ToggledText
{
    [JsonPropertyName("text")]
    public string Text
    {
        get; set;
    }
}
