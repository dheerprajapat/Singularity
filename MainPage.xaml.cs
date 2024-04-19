using CommunityToolkit.Maui.Views;

namespace Singularity
{
    public partial class MainPage : ContentPage
    {
#nullable disable
        public static MediaElement MediaElement { get; private set; }
#nullable restore
        public MainPage()
        {
            InitializeComponent();
            MediaElement = MediaUIElement;
        }
    }
}
