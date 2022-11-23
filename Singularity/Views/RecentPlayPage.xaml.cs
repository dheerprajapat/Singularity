using Microsoft.UI.Xaml.Controls;

using Singularity.ViewModels;

namespace Singularity.Views;

public sealed partial class RecentPlayPage : Page
{
    public RecentPlayViewModel ViewModel
    {
        get;
    }

    public RecentPlayPage()
    {
        ViewModel = App.GetService<RecentPlayViewModel>();
        InitializeComponent();
    }
}
