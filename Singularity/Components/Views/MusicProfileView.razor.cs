using Microsoft.AspNetCore.Components;
using Singularity.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CommunityToolkit.Maui.Storage;
using System.Threading;
using Microsoft.Extensions.Logging;
using System.Net;
using System.ComponentModel;
using AngleSharp.Dom;
using Singularity.Managers;

namespace Singularity.Components.Views
{
    partial class MusicProfileView : IDisposable
    {
        [Parameter]
        public IAsyncEnumerable<ISong>? Songs { get; set; }
        [Inject]
        public DownloadManager? DownloadManager { get; set; }

        private IAsyncEnumerator<ISong>? songEnumerator;
        public List<ISong> ResizableSongsList = new List<ISong>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (Songs == null) return;
            songEnumerator = Songs?.GetAsyncEnumerator();
            DownloadManager.DownloadProgressChanged += DownloadManager_DownloadProgressChanged;
        }

        private void DownloadManager_DownloadProgressChanged(object? sender, DownloadProgressEventArgs e)
        {
            Debug.WriteLine(e.Percentage);
        }

        private async void DownloadSongs()
        {
            try
            {
                var folderPath = FileSystem.Current.AppDataDirectory;
                await Task.Run(async () =>
                {
                    while (await songEnumerator.MoveNextAsync())
                    {
                        var currentSong = songEnumerator.Current;
                        await DownloadManager!.DownloadSongAsync(currentSong);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void PlaySongsList()
        {

        }

        public void Dispose()
        {
            DownloadManager.DownloadProgressChanged -= DownloadManager_DownloadProgressChanged;
        }
    }
}
