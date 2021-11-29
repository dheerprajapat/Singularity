using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SonicAudioApp.Components
{
    public sealed partial class MediaPlayerControl : UserControl
    {
        public MediaPlayerControl()
        {
            this.InitializeComponent();
        }
        private Dictionary<UIElement, Brush> PreviousColors = new Dictionary<UIElement, Brush>();
        private void FontIcon_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var icon = sender as FontIcon;
            if (!PreviousColors.ContainsKey(icon))
                PreviousColors.Add(icon, icon.Foreground);

            if (icon.Name == "LikeButton")
                icon.Foreground = new SolidColorBrush(Colors.Green);
            else
                icon.Foreground = new SolidColorBrush(Colors.DeepSkyBlue);
        }

        private void FontIcon_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var icon = sender as FontIcon;
            icon.Foreground = PreviousColors.ContainsKey(icon) ? PreviousColors[icon] : new SolidColorBrush(Colors.White);
        }
    }
}
