using Singularity.Contracts.Services;
using Singularity.Core.Contracts.Services;
using Singularity.Helpers;
using Singularity.Views;

namespace Singularity;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();
        App.GetService<DiscordPresenceService>().Initialize();
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();
    }

    private void WindowEx_Closed(object sender, Microsoft.UI.Xaml.WindowEventArgs args)
    {
        var settingsService = App.GetService<IUserSettingsService>();
        settingsService.Write(settingsService.CurrentSetting);
        MusicControllerView.ExViewModel?.playerElement?.MediaPlayer?.Pause();
    }
}
