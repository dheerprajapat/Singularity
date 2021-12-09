using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace SonicAudioApp.Models
{
    public class PageIntent
    {
        public Page FromPage { get; set; }
        public object Data { get; set; }
        public PlayListType Type { get; set; }
        public string Url { get; set; }
    }
    public enum PlayListType
    {
        Youtube,
        Liked
    }
}
