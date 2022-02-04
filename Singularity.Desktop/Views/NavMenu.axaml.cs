using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Singularity.Desktop.Views;

public class NavMenu : UserControl
{
    public NavMenu()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}