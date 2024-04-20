using CommunityToolkit.Maui.Views;

namespace Singularity
{
    public partial class MainPage : ContentPage
    {
#nullable disable
        public static MediaElement MediaElement { get; private set; }
        public static Func<bool> OnBackButtonPress { get; set; }
#nullable restore
        public MainPage()
        {
            InitializeComponent();
            MediaElement = MediaUIElement;
        }

        protected override bool OnBackButtonPressed()
        {
            if(OnBackButtonPressed == null)
                return base.OnBackButtonPressed();
            return OnBackButtonPress.Invoke();
        }
    }
}
