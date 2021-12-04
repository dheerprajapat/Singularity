using Newtonsoft.Json.Linq;
using SonicAudioApp.Services.YoutubeSearch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace SonicAudioApp.Models;

public record AudioQueueItem:INotifyPropertyChanged
{
    public string Url { get;set; }
    public string Title { get; set; }
    public string Singers { get; set; }
    public string ThumbnailUrl { get; set; }
    public string Id { get; set; }
    public string VideoUrl { get; set; }
    private bool liked = false;
    public bool Liked
    {
        get => liked;
        set
        {
            liked = value;
            NotifyPropertyChanged();
        }
    }
    public string DurationString { get; set; }

    private Visibility waveformVisibilty=Visibility.Collapsed;
    public Visibility WaveformVisibilty {
        get => waveformVisibilty;
        set 
        { 
            waveformVisibilty = value;
            NotifyPropertyChanged();
        }
    }
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;
}