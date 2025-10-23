using Paftax.Pafta.Shared.Models;
using System.Collections.ObjectModel;

namespace Paftax.Pafta.Shared.Interfaces
{
    public interface IExportScheduleViewModel
    {
        ObservableCollection<ScheduleModel> Schedules { get; }
        bool IsSeperated { get; set; }
        bool IsMerged { get; set; }
        string ExportFolderPath { get; set; }

        event Action? CloseAction;
        event Action? ExportAction;
    }
}
