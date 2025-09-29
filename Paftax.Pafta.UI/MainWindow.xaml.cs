using Paftax.Pafta.UI.ViewModels;
using System.Windows;

namespace Paftax.Pafta.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool ShowCloseButton { get; set; } = true;
        public bool ShowMaximizeButton { get; set; } = false;
        public bool ShowMinimizeButton { get; set; } = false;
        public bool ShowHelpButton { get; set; } = true;
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                TitleBar.CloseButton = ShowCloseButton;
                TitleBar.MaximizeButton = ShowMaximizeButton;
                TitleBar.MinimizeButton = ShowMinimizeButton;
                TitleBar.HelpButton = ShowHelpButton;
            };

            if (DataContext is MainViewModel vm)
            {
                vm.CloseRequest += () => Close();
            }
        }
    }
}
