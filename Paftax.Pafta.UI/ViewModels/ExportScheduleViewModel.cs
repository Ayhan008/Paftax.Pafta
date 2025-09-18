using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Paftax.Pafta.UI.Models;
using System.Collections.ObjectModel;

namespace Paftax.Pafta.UI.ViewModels
{
    internal partial class ExportScheduleViewModel : ObservableObject
    {
        private readonly List<ScheduleViewModel> allSchedules = [];

        public ObservableCollection<ScheduleViewModel> ScheduleViewModels { get; } = [];

        #region Observable Properties
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
            ApplyFilter();
        }
        #endregion

        public event Action? RequestClose;

        [RelayCommand]
        private void Cancel() => RequestClose?.Invoke();

        [RelayCommand]
        private void Export() => RequestClose?.Invoke();

        public void LoadSchedules(IEnumerable<ScheduleModel>? models)
        {
            allSchedules.Clear();
            ScheduleViewModels.Clear();
            if (models == null) return;

            foreach (var model in models)
            {
                var vm = new ScheduleViewModel(model);
                allSchedules.Add(vm);
                ScheduleViewModels.Add(vm);
            }
        }

        public List<ScheduleModel> GetSelectedSchedules()
        {
            return [.. ScheduleViewModels
                .Where(vm => vm.IsChecked)
                .Select(vm => vm.Model)];
        }

        private void ApplyFilter()
        {
            ScheduleViewModels.Clear();

            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? allSchedules
                : allSchedules.Where(x =>
                      x.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            foreach (var vm in filtered)
                ScheduleViewModels.Add(vm);
        }
    }
}
