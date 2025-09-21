using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Paftax.Pafta.UI.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class ExportScheduleViewModel : BaseViewModel
    {
        private readonly List<ScheduleViewModel> _loadedSchedules = [];
        private List<ScheduleModel> _selectedSchedules = [];
        public ObservableCollection<ScheduleViewModel> ScheduleViewModels { get; } = [];

        public bool CanExport
        {
            get
            {
                return (ScheduleViewModels.Any(vm => vm.IsChecked) &&
                        !string.IsNullOrWhiteSpace(ExportFolderPath) &&
                        (IsMerged || IsSeperated));
            }
        }

        public string? HelpText =
            "Select at least one schedule from the list," +
            "then choose your export options." +
            "Specify the folder path and select" +
            " the spreadsheet export method." +
            "'Merged' generates a single Excel file" +
            "containing all selected schedules." +
            "'Separated' creates one Excel file" +
            "per selected schedule.";

        [ObservableProperty]
        private bool isAllChecked = false;

        partial void OnIsAllCheckedChanged(bool value)
        {
            foreach (var item in ScheduleViewModels)
            {
                if (item.IsChecked != value)
                    item.IsChecked = value;
            }
        }

        [ObservableProperty]
        private string searchText = string.Empty;

        partial void OnSearchTextChanged(string value)
        {
            ScheduleViewModels.Clear();

            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _loadedSchedules
                : _loadedSchedules.Where(x =>
                      x.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            foreach (var vm in filtered)
                ScheduleViewModels.Add(vm);
        }

        [ObservableProperty]
        private bool isMerged = false;
        partial void OnIsMergedChanged(bool value)
        {
            if (value)
                IsSeperated = false;
            ExportCommand.NotifyCanExecuteChanged();
        }


        [ObservableProperty]
        private bool isSeperated = true;
        partial void OnIsSeperatedChanged(bool value)
        {
            if (value)
                IsMerged = false;
            ExportCommand.NotifyCanExecuteChanged();
        }


        [ObservableProperty]
        private string exportFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

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
            OnCloseRequest();
        }

        public event Action? RequestExport;

        [RelayCommand(CanExecute = nameof(CanExport))]
        private void Export()
        {
            GetSelectedSchedules();

            if (!string.IsNullOrWhiteSpace(ExportFolderPath) && !Directory.Exists(ExportFolderPath))
                Directory.CreateDirectory(ExportFolderPath);

            var selectedSchedules = GetSelectedSchedules();

            if (selectedSchedules.Count > 0)
            {
                RequestExport?.Invoke();
                OnCloseRequest();
            }         
        }

        public void LoadSchedules(List<ScheduleModel> models)
        {
            _loadedSchedules.Clear();
            ScheduleViewModels.Clear();

            if (models == null)
                return;

            foreach (var model in models)
            {
                ScheduleViewModel vm = new(model);
                vm.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ScheduleViewModel.IsChecked))
                        ExportCommand.NotifyCanExecuteChanged();
                };
                _loadedSchedules.Add(vm);
                ScheduleViewModels.Add(vm);
            }
        }

        public List<ScheduleModel> GetSelectedSchedules()
        {
            _selectedSchedules = [.. ScheduleViewModels
                .Where(vm => vm.IsChecked)
                .Select(vm => vm.Model)];

            return _selectedSchedules;
        }
    }
}
