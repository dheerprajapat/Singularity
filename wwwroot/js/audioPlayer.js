let audio;
export function createAudio(src,dotnet)
{
    if (src != undefined && src != null)
        audio = new Audio(src);
    else
        audio = new Audio();

    audio.oncanplay = () => dotnet.invokeMethodAsync("oncanplay");
    audio.ontimeupdate = () => dotnet.invokeMethodAsync("ontimeupdate");
    audio.onloadedmetadata = () => dotnet.invokeMethodAsync("onloadedmetadata");
    audio.onended = () => dotnet.invokeMethodAsync("onended");
}

export function setSrc(src) {
    audio.src = src;
}

export function pause() {
    audio.pause();
}

export function play() {
    audio.play();
}

export function duration() {
    return audio.duration != undefined ? audio.duration : 0;
}

export function getCurrentTime()
{
    return audio.currentTime;
}
export function setCurrentTime(val) {
    audio.currentTime= val;
}

export function isPaused() {
    return audio.paused;
}

export function getVolume() {
    return audio.volume;
}

export function setVolume(value) {
    audio.volume = value;
}

export function isMuted() {
    return audio.muted;
}

export function setMuted(value) {
    audio.muted = value;
}
export function getReadyState() {
    return audio.readyState;
}