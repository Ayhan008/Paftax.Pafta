using System.Windows;

namespace Paftax.Pafta.UI.Test
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.ProgressRingDemoViewModel();
        }
    }
}
