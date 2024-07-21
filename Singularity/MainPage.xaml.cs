using Singularity.Services;

namespace Singularity;

public partial class MainPage : ContentPage
{
#nullable disable
    public static MainPage Current { get; private set; }
#nullable restore

    public MainPage()
    {
        InitializeComponent();
        Current = this;
        AudioManager.InitMediaElement(mediaElement);
        this.Unloaded += MainPageUnloaded;
    }

    private void MainPageUnloaded(object? sender, EventArgs e)
    {
        DisposeMediaElement();
    }

    internal void DisposeMediaElement()
    {
        mediaElement.Dispose();
        // Stop and cleanup MediaElement when we navigate away
        mediaElement.Handler?.DisconnectHandler();
        Application.Current?.Quit();

    }
}
