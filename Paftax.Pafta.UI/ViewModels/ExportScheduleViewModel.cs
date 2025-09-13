using System.Collections.ObjectModel;

namespace Paftax.Pafta.UI.ViewModels
{
    internal class ExportScheduleViewModel
    {
        public ObservableCollection<ScheduleViewModel> ScheduleViewModels { get; set; } = [];
    }
}
