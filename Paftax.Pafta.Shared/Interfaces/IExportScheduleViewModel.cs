using Paftax.Pafta.Shared.Models;
using System.Collections.ObjectModel;

namespace Paftax.Pafta.Shared.Interfaces
{
    public interface IExportScheduleViewModel
    {
        bool IsReadyForExport { get; set; }
        bool IsMerged { get; set; }
        bool IsSeperated { get; set; }
        string ExportFolderPath { get; set; }
        ObservableCollection<ScheduleModel> Schedules { get; }
        void LoadSchedules(List<ScheduleModel> scheduleModels);
        List<ScheduleModel> SelectedSchedules();

        event Action? CloseRequest;
        event Action? RequestExport;
    }
}
