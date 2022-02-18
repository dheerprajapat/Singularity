using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFUI.Controls;

namespace SingularityDesktop.Win
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WPFUI.Background.Manager.Apply(this);
        }
        private void RootNavigation_OnLoaded(object sender, RoutedEventArgs e)
        {
            RootNavigation.Navigate("home");
        }

        private void RootDialog_LeftButtonClick(object sender, RoutedEventArgs e)
        {
            Log.Debug("Root dialog action button was clicked!");
        }

        private void RootDialog_RightButtonClick(object sender, RoutedEventArgs e)
        {
            Log.Debug("Root dialog custom right button was clicked!");

        }

        private void RootNavigation_OnNavigated(object sender, RoutedEventArgs e)
        {
            Log.Debug("Page now is: " + (sender as NavigationFluent)?.PageNow);
        }

        private void TitleBar_OnMinimizeClicked(object sender, RoutedEventArgs e)
        {
            Log.Debug("Minimize button clicked");
        }

        private void TrayMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem menuItem) return;

            string tag = menuItem.Tag as string ?? String.Empty;

            Log.Debug("Menu item clicked: " + tag);
        }

        private void RootTitleBar_OnNotifyIconClick(object sender, RoutedEventArgs e)
        {
            Log.Debug("Notify Icon clicked");
        }

        private void RootNavigation_OnNavigatedForward(object sender, RoutedEventArgs e)
        {
            Log.Debug("Navigated forward");
        }

        private void RootNavigation_OnNavigatedBackward(object sender, RoutedEventArgs e)
        {
            Log.Debug("Navigated backward");
        }
    }
}
