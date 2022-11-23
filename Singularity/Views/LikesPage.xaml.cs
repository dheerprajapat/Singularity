using Microsoft.UI.Xaml.Controls;

using Singularity.ViewModels;

namespace Singularity.Views;

public sealed partial class LikesPage : Page
{
    public LikesViewModel ViewModel
    {
        get;
    }

    public LikesPage()
    {
        ViewModel = App.GetService<LikesViewModel>();
        InitializeComponent();
    }
}
