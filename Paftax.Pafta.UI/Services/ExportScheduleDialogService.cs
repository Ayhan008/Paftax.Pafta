using Paftax.Pafta.Shared.Models;
using Paftax.Pafta.UI.Dialogs;
using Paftax.Pafta.UI.ViewModels;
using System.Windows;

namespace Paftax.Pafta.UI.Services
{
    public class ExportScheduleDialogService
    {
        private readonly ExportScheduleViewModel _exportScheduleViewModel = new();
        public void LoadSchedules(IEnumerable<ScheduleModel> scheduleModels)
        {
            _exportScheduleViewModel.LoadSchedules(scheduleModels);
        }
        public Window ShowDialog()
        {
            DialogOptions dialogOptions = new()
            {
                Title = "Export Schedule",
                Width = 500,
                Height = 800
            };

            Window window = CommandDialog.Show(_exportScheduleViewModel, dialogOptions);
            return window;
        }
        public event Action? ExportAction
        {
            add => _exportScheduleViewModel.ExportAction += value;
            remove => _exportScheduleViewModel.ExportAction -= value;
        }
        public bool IsSeperated { get => _exportScheduleViewModel.IsSeperated; }
        public bool IsMerged { get => _exportScheduleViewModel.IsMerged; }
        public string ExportFolderPath { get => _exportScheduleViewModel.ExportFolderPath; }
        public List<ScheduleModel> SelectedSchedules { get => [.. _exportScheduleViewModel.Schedules.Where(schedule => schedule.IsChecked)]; }
    }
}