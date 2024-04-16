using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;

namespace Singularity.Audio
{
    internal class AudioPlayer
    {
        IJSObjectReference audioJSReference;
        private string src;
        private AudioPlayer(IJSObjectReference audioJSReference,string src)
        {
            this.audioJSReference = audioJSReference;
            this.src = src;
        }
        public static async Task<AudioPlayer> CreateAsync(IJSRuntime runtime,string src=null)
        {
            var audioJSObj = await runtime.InvokeAsync<IJSObjectReference>("import","./js/audioPlayer.js");
            var player = new AudioPlayer(audioJSObj,src);
            await player.CreateInternalAsync();
            return player;
        }
        private ValueTask CreateInternalAsync()
        {
            return audioJSReference.InvokeVoidAsync("createAudio",src,DotNetObjectReference.Create(this));
        }

        public ValueTask SetSrc(string src)
        {
            return audioJSReference.InvokeVoidAsync("setSrc",src);
        }

        public ValueTask Play()
        {
            return audioJSReference.InvokeVoidAsync("play");
        }

        public ValueTask Pause()
        {
            return audioJSReference.InvokeVoidAsync("pause");
        }

        public ValueTask<double> GetDuration()
        {
            return audioJSReference.InvokeAsync<double>("duration");
        }

        public ValueTask<double> GetCurrentTime()
        {
            return audioJSReference.InvokeAsync<double>("getCurrentTime");
        }
        public ValueTask SetCurrentTime(double time)
        {
            return audioJSReference.InvokeVoidAsync("setCurrentTime",time);
        }
        public ValueTask<double> GetVolume()
        {
            return audioJSReference.InvokeAsync<double>("getVolume");
        }
        public ValueTask SetVolume(double volume)
        {
            return audioJSReference.InvokeVoidAsync("setVolume", volume);
        }
        public ValueTask<bool> IsPaused()
        {
            return audioJSReference.InvokeAsync<bool>("isPaused");
        }
        public ValueTask<bool> isMuted()
        {
            return audioJSReference.InvokeAsync<bool>("isMuted");
        }
        public ValueTask SetMuted(bool muted)
        {
            return audioJSReference.InvokeVoidAsync("setMuted", muted);
        }

        public async ValueTask<ReadyStates> GetReadyState()
        {
            return (ReadyStates)(await audioJSReference.InvokeAsync<int>("getReadyState"));
        }

        [JSInvokable("oncanplay")]
        public void CanPlay()
        {
            OnCanPlay?.Invoke(this);
        }

        [JSInvokable("ontimeupdate")]
        public void TimeUpdate()
        {
            OnTimeUpdate?.Invoke(this);
        }

        [JSInvokable("onloadedmetadata")]
        public void LoadedMetadata()
        {
            OnLoadedMetadata?.Invoke(this);
        }

        [JSInvokable("onended")]
        public void Ended()
        {
            OnEnded?.Invoke(this);
        }

        public delegate void AudioEventHandler(object sender);
        public event AudioEventHandler? OnCanPlay;
        public event AudioEventHandler? OnTimeUpdate;
        public event AudioEventHandler? OnLoadedMetadata;
        public event AudioEventHandler? OnEnded;


    }
    public enum ReadyStates
    {
        HaveNothing = 0,
        HaveMetadata = 1,
        HaveCurrentData = 2,
        HaveFutureData = 3,
        HaveEnoughData
    }
}
