using Microsoft.UI.Xaml.Controls;

using Singularity.ViewModels;

namespace Singularity.Views;

public sealed partial class SearchPage : Page
{
    public SearchViewModel ViewModel
    {
        get;
    }

    public SearchPage()
    {
        ViewModel = App.GetService<SearchViewModel>();
        InitializeComponent();
    }
}
