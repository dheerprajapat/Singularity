using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Singularity.Desktop
{
    public partial class MainLayout : UserControl
    {
        public MainLayout()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
