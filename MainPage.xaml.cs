using CommunityToolkit.Maui.Views;

namespace Singularity
{
    public partial class MainPage : ContentPage
    {
#nullable disable
        public static MediaElement MediaElement { get; private set; }


#nullable restore
        public delegate bool BackButtonHandler();
        public static event BackButtonHandler? OnBackButtonPress;

        public MainPage()
        {
            InitializeComponent();
            MediaElement = MediaUIElement;
        }

        protected override bool OnBackButtonPressed()
        {
            if(OnBackButtonPressed == null)
                return base.OnBackButtonPressed();

            foreach(var func in OnBackButtonPress!.GetInvocationList())
            {
                if ((bool)func.DynamicInvoke())
                    return true;
            }    
            return false;
        }
    }
}
