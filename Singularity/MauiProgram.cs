﻿using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Singularity.Contracts;
using Singularity.Services;

namespace Singularity
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMediaElement()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })
                .ConfigureLifecycleEvents(events =>
                {
#if ANDROID
                    //events.AddAndroid(android => android.OnDestroy(e=>
                    //{
                    //    MainPage.Current.DisposeMediaElement();
                    //}));
#endif
                });
            builder.Services.AddSingleton<IMusicHub,YoutubeMusicHub>();
            builder.Services.AddTransient<IAuthenticatonService, FirebaseAuthService>();
            builder.Services.AddTransient<IDatabaseService, FirestoreDbService>();

            builder.Services.AddSingleton<AudioManager>();

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
