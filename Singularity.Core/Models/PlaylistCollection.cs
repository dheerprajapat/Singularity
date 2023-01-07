using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Singularity.Core.Models;
public class PlaylistCollection
{

}
public class PlaylistItem
{
    public PlaylistItemType PlaylistType = PlaylistItemType.Custom;
    [JsonIgnore]
    public  bool IsReadOnly => PlaylistType == PlaylistItemType.Default;
    public string Name
    {
        get;set;
    }
}

public enum PlaylistItemType
{
    /// <summary>
    /// youtubes default playlist
    /// </summary>
    Default,
    /// <summary>
    /// Includes songs from outside youtube's builtin playlist
    /// </summary>
    Custom 
}