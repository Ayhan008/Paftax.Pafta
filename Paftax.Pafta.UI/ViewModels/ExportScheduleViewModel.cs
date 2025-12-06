using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Paftax.Pafta.Shared.Models.Element;
using System.IO;

namespace Paftax.Pafta.UI.ViewModels
{
    public partial class ExportScheduleViewModel<T>(SearchableElementCollectionViewModel<T> scheduleSelectionViewModel) : ObservableObject where T : ElementModel
    {
        public SearchableElementCollectionViewModel<T> ScheduleSelectionViewModel { get; } = scheduleSelectionViewModel;

        [ObservableProperty]
        private bool isMerged = false;

        [ObservableProperty]
        private bool isSeperated = true;

        [ObservableProperty]
        private string exportFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public event Action? CloseAction;
        public event Action? ExportAction;

        private bool CanExport()
        {
            var hasSelection = ScheduleSelectionViewModel?.Elements?.Any(s => s.IsChecked) == true;
            var hasValidMode = IsMerged || IsSeperated;
            var hasFolder = !string.IsNullOrWhiteSpace(ExportFolderPath);
            return hasSelection && hasValidMode && hasFolder;
        }

        partial void OnIsMergedChanged(bool value)
        {
            if (value)
            {
                IsSeperated = false;
            }
            ExportCommand.NotifyCanExecuteChanged();
        }

        partial void OnIsSeperatedChanged(bool value)
        {
            if (value)
            {
                IsMerged = false;
            }
            ExportCommand.NotifyCanExecuteChanged();
        }

        partial void OnExportFolderPathChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string trimmedString = value.Trim('"');

                if (trimmedString != value)
                {
                    ExportFolderPath = trimmedString;
                }
            }
            ExportCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private void Cancel() => CloseAction?.Invoke();

        [RelayCommand(CanExecute = nameof(CanExport))]
        private void Export()
        {
            if (!string.IsNullOrWhiteSpace(ExportFolderPath) && !Directory.Exists(ExportFolderPath))
            {
                Directory.CreateDirectory(ExportFolderPath);
            }

            var anyChecked = ScheduleSelectionViewModel?.Elements?.Any(s => s.IsChecked) == true;
            if (anyChecked)
            {
                ExportAction?.Invoke();
                CloseAction?.Invoke();
                ExportCommand.NotifyCanExecuteChanged();
            }
        }
    }
}