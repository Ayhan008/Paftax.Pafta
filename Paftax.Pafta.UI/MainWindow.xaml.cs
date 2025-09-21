using Paftax.Pafta.UI.ViewModels;
using System.Windows;

namespace Paftax.Pafta.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(BaseViewModel vm)
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel
            {
                CurrentViewModel = vm
            };

            vm.CloseRequest += () => this.Close();
        }
    }
}
