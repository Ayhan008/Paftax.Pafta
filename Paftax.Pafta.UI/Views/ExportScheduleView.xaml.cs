using Paftax.Pafta.UI.Models;
using Paftax.Pafta.UI.ViewModels;
using System.Windows;

namespace Paftax.Pafta.UI.Views
{
    /// <summary>
    /// Interaction logic for ExportScheduleView.xaml
    /// </summary>
    public partial class ExportScheduleView : Window
    {
        public ExportScheduleView(List<ScheduleModel> scheduleModels)
        {
            InitializeComponent();
            ExportScheduleViewModel exportScheduleViewModel = new();
            exportScheduleViewModel.LoadSchedules(scheduleModels);

            DataContext = exportScheduleViewModel;
        }

        private void SearchBox_SearchTextChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
