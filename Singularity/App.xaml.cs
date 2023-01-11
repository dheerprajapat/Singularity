using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

using Singularity.Activation;
using Singularity.Contracts.Services;
using Singularity.Core.Contracts.Services;
using Singularity.Core.Services;
using Singularity.Helpers;
using Singularity.Models;
using Singularity.Notifications;
using Singularity.Services;
using Singularity.ViewModels;
using Singularity.Views;

namespace Singularity;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers
            services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();

            // Services
            services.AddSingleton<IAppNotificationService, AppNotificationService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IYoutubeService, YoutubeExplodeService>();
            services.AddSingleton<IUserSettingsService, UserSettingsService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            services.AddTransient<HomePage>();
            services.AddTransient<HomeViewModel>();

            services.AddTransient<LikesViewModel>();
            services.AddTransient<LikesPage>();

            services.AddTransient<SearchViewModel>();
            services.AddTransient<SearchPage>();

            services.AddTransient<PlaylistViewModel>();
            services.AddTransient<PlaylistPage>();

            services.AddTransient<RecentPlayViewModel>();
            services.AddTransient<RecentPlayPage>();

            services.AddTransient<MusicCotrollerViewModel>();
            services.AddTransient<MusicControllerView>();

            services.AddTransient<SearchItemFragmentView>();
            services.AddTransient<SearchItemFragmentViewModel>();

            services.AddTransient<SongStringCollectionPage>();
            services.AddTransient<SongStringCollectionPageViewModel>();

            services.AddTransient<SearchItemView>();
            services.AddTransient<SearchItemViewModel>();


            services.AddTransient<PlaylistItemPage>();
            services.AddTransient<PlaylistItemPageViewModel>();

            services.AddTransient<ChannelItemPage>();
            services.AddTransient<ChannelItemPageViewModel>();

            services.AddTransient<HomeQuickPlayViewModel>();
            services.AddTransient<HomeQuickPlayView>();

            services.AddTransient<VideoIdListView>();
            services.AddTransient<VideoIdListViewModel>();

            services.AddTransient<BannerView>();
            services.AddTransient<BannerViewModel>();

            services.AddTransient<LocalCustomSongCollectionView>();


            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        App.GetService<IAppNotificationService>().Initialize();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        //App.GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
