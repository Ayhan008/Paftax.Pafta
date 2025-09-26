using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Paftax.Pafta.Shared.Interfaces;
using Paftax.Pafta.Shared.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class ExportScheduleViewModel : ObservableObject, IExportScheduleViewModel
    {
        private readonly List<ScheduleModel> _loadedSchedules = [];
        public ObservableCollection<ScheduleModel> Schedules { get; } = [];
        public event Action? CloseRequest;
        public event Action? RequestExport;

        [ObservableProperty]
        private bool isReadyForExport = false;
        [ObservableProperty]
        private bool isAllChecked = false;
        [ObservableProperty]
        private string searchText = string.Empty;
        [ObservableProperty]
        private bool isMerged = false;
        [ObservableProperty]
        private bool isSeperated = true;
        [ObservableProperty]
        private string exportFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public bool CanExport
        {
            get
            {
                return (Schedules.Any(schedule => schedule.IsChecked) &&
                        !string.IsNullOrWhiteSpace(ExportFolderPath) &&
                        (IsMerged || IsSeperated));
            }
        }

        partial void OnIsAllCheckedChanged(bool value)
        {
            foreach (var item in Schedules)
            {
                if (item.IsChecked != value)
                    item.IsChecked = value;
            }
        }

        partial void OnSearchTextChanged(string value)
        {
            Schedules.Clear();

            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _loadedSchedules
                : _loadedSchedules.Where(x =>
                      x.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            foreach (ScheduleModel scheduleModel in filtered)
                Schedules.Add(scheduleModel);
        }
      
        partial void OnIsMergedChanged(bool value)
        {
            if (value)
                IsSeperated = false;
            ExportCommand.NotifyCanExecuteChanged();
        }

        partial void OnIsSeperatedChanged(bool value)
        {
            if (value)
                IsMerged = false;
            ExportCommand.NotifyCanExecuteChanged();
        }

        partial void OnExportFolderPathChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string trimmedString = value.Trim('"');

                if (trimmedString != value)
                    ExportFolderPath = trimmedString;
            }
            ExportCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private void Cancel()
        {
            IsReadyForExport = false;
            CloseRequest?.Invoke();
        }

        [RelayCommand(CanExecute = nameof(CanExport))]
        private void Export()
        {
            SelectedSchedules();

            if (!string.IsNullOrWhiteSpace(ExportFolderPath) && !Directory.Exists(ExportFolderPath))
                Directory.CreateDirectory(ExportFolderPath);

            var selectedSchedules = SelectedSchedules();

            if (selectedSchedules.Count > 0)
            {
                IsReadyForExport = true;
                RequestExport?.Invoke();
                CloseRequest?.Invoke();

                ExportCommand.NotifyCanExecuteChanged();
            }
        }

        public void LoadSchedules(List<ScheduleModel> scheduleModels)
        {
            _loadedSchedules.Clear();
            Schedules.Clear();

            _loadedSchedules.AddRange(scheduleModels);

            foreach (var schedule in _loadedSchedules)
            {
                schedule.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(schedule.IsChecked))
                        ExportCommand.NotifyCanExecuteChanged();
                };
                Schedules.Add(schedule);
            }                    
        }

        public List<ScheduleModel> SelectedSchedules()
        {
            return [.. _loadedSchedules.Where(s => s.IsChecked)];
        }
    }
}
