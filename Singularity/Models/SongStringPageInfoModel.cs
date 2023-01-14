using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.Models;
public class SongStringPageInfoModel
{
    public string? Title { get; set; }

    public string? Author
    {
        get; set;
    }

    public string? Thumbnail
    {
        get; set;
    }

    public ObservableCollection<string>? Items{ get; set; }
    public string? PlaylistName
    {
        get;set;
    }
}
