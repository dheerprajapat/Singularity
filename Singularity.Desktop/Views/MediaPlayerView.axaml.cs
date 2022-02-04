using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Singularity.Desktop.Views;

public class MediaPlayerView : UserControl
{
    public MediaPlayerView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}