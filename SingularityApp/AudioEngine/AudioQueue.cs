using SonicAudioApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicAudioApp.AudioEngine;
public static class AudioQueue
{
    public static ObservableCollection<AudioQueueItem> Queue { get; } = new();

    public static void Add(AudioQueueItem song)
    {
        if(Queue.Contains(song))
        {
            Queue.Remove(song);
        }
        Queue.Add(song);

    }
    public static async Task AddAndPlayAsync(AudioQueueItem song)
    {
        if (Queue.Contains(song))
        {
            Queue.Remove(song);
        }
        Queue.Insert(0, song);

        await AudioPlayer.PlayAsync(true);
    }
    public static void InsertAt(AudioQueueItem song,int ind=1)
    {
        if (Queue.Contains(song))
        {
            Queue.Remove(song);
        }
        Queue.Insert(ind, song);

    }

    public static AudioQueueItem? Current => Queue.Count > 0 ? Queue[0] : null;
    public static AudioQueueItem? Next()
    {
        if(Repeat==LoopMode.NoLoop && Queue.Count>=1)
                Queue.RemoveAt(0);
        else if(Repeat==LoopMode.LoopAll && Queue.Count >= 1)
        {
            var item = Queue[0];
            Queue.RemoveAt(0);
            Add(item);
        }

        return Current;
    }

    public static int Count=> Queue.Count;

    public static AudioQueueItem? Previous()
    {
        if(Queue.Count>=1)
        {
            if (AudioPlayer.Position > TimeSpan.FromSeconds(5))
            {
                AudioPlayer.UpdatePosition(TimeSpan.FromSeconds(0));
            }
            else
            {
                var last = Queue[Queue.Count - 1];
                Queue.RemoveAt(Queue.Count - 1);
                Queue.Insert(0, last);
            }
        }

        return Current;
    }

    public static LoopMode Repeat = LoopMode.LoopAll;

    public static void Remove(AudioQueueItem d)
    {
        if(Current==d)
        {
            AudioPlayer.Stop();
        }
        int ind=Queue.IndexOf(d);
        if (ind == -1)
            return;
        Queue.RemoveAt(ind);
    }
}
public enum LoopMode
{
    NoLoop,
    LoopSingle,
    LoopAll
}