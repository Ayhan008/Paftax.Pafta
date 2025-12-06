using Paftax.Pafta.Shared.Models.Element;
using Paftax.Pafta.UI.ViewModels;
using System.Windows;

namespace Paftax.Pafta.UI.Views
{
    public partial class ExportScheduleWindow : Window
    {
        public ExportScheduleWindow(ExportScheduleViewModel<ScheduleModel> exportScheduleViewModel)
        {
            InitializeComponent();
            DataContext = exportScheduleViewModel;
        }
    }
}
