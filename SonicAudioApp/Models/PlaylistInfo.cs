using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SonicAudioApp.Models
{
    public record PlaylistInfo: INotifyPropertyChanged
    {
        public string Title { get; set; }
        public string Thumbnail { get; set; } = @"../Assets/playlist_logo.jpg";
        public string Author { get; set; }
        private ObservableCollection<AudioQueueItem> songs=new();

        public ObservableCollection<AudioQueueItem> Songs
        {
            get { return songs; }
            set { 
                if (value == null) return;
                if(songs!=null)
                    songs.CollectionChanged -= Songs_CollectionChanged;  
                songs = value;
                songs.CollectionChanged += Songs_CollectionChanged;
                 }
        }

        private void Songs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged(string.Empty);
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
}
