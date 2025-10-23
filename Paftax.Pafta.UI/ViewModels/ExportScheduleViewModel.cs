using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Paftax.Pafta.Shared.Interfaces;
using Paftax.Pafta.Shared.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class ExportScheduleViewModel : ObservableObject, IExportScheduleViewModel
    {
        public ObservableCollection<ScheduleModel> Schedules { get; } = [];
        public ICollectionView SchedulesView { get; }

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

        public event Action? CloseAction;
        public event Action? ExportAction;

        public ExportScheduleViewModel()
        {
            SchedulesView = CollectionViewSource.GetDefaultView(Schedules);
            SchedulesView.Filter = FilterSchedules;
        }

        public bool CanExport
        {
            get
            {
                return (Schedules.Any(schedule => schedule.IsChecked) &&
                        !string.IsNullOrWhiteSpace(ExportFolderPath) &&
                        (IsMerged || IsSeperated));
            }
        }

        private bool FilterSchedules(object obj)
        {
            if (obj is ScheduleModel schedule)
            {
                return string.IsNullOrWhiteSpace(SearchText)
                    || schedule.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        partial void OnIsAllCheckedChanged(bool value)
        {
            foreach (ScheduleModel scheduleModel in Schedules)
            {
                if (scheduleModel.IsChecked != value)
                    scheduleModel.IsChecked = value;
            }
        }

        partial void OnSearchTextChanged(string value)
        {
            SchedulesView.Refresh();
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
            CloseAction?.Invoke();
        }

        [RelayCommand(CanExecute = nameof(CanExport))]
        private void Export()
        {
            if (!string.IsNullOrWhiteSpace(ExportFolderPath) && !Directory.Exists(ExportFolderPath))
                Directory.CreateDirectory(ExportFolderPath);

            if (Schedules.Where(s => s.IsChecked == true).ToList().Count > 0)
            {
                ExportAction?.Invoke();
                CloseAction?.Invoke();
                ExportCommand.NotifyCanExecuteChanged();
            }
        }

        public void LoadSchedules(IEnumerable<ScheduleModel> scheduleModels)
        {
            Schedules.Clear();
            foreach (ScheduleModel scheduleModel in scheduleModels)
            {
                scheduleModel.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(scheduleModel.IsChecked))
                    {
                        ExportCommand.NotifyCanExecuteChanged();
                    }
                        
                };
                Schedules.Add(scheduleModel);
            }
        }
    }
}
