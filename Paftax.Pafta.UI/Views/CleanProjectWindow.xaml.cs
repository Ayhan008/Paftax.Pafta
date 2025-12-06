using Paftax.Pafta.Shared.Models;
using Paftax.Pafta.UI.ViewModels;
using System.Windows;

namespace Paftax.Pafta.UI.Views
{
    public partial class CleanProjectWindow : Window
    {
        public CleanProjectWindow(CleanProjectViewModel<GarbageElementModel> cleanProjectViewModel)
        {
            InitializeComponent();
            DataContext = cleanProjectViewModel;
        }
    }
}
