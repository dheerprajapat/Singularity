using Singularity.Core.Contracts.Services;
using Singularity.Helpers;

namespace Singularity;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();
    }

    private void WindowEx_Closed(object sender, Microsoft.UI.Xaml.WindowEventArgs args)
    {
        var settingsService = App.GetService<IUserSettingsService>();
        settingsService.Write(settingsService.CurrentSetting);
    }
}
