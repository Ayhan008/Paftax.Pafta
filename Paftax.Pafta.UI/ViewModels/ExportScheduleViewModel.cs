using Paftax.Pafta.UI.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Paftax.Pafta.UI.ViewModels
{
    internal class ExportScheduleViewModel() : INotifyPropertyChanged
    {
        public ObservableCollection<ScheduleViewModel> ScheduleViewModels { get; set; } = [];

        private bool _isAllChecked;
        public bool IsAllChecked
        {
            get => _isAllChecked;
            set
            {
                if (_isAllChecked != value)
                {
                    _isAllChecked = value;
                    OnPropertyChanged(nameof(IsAllChecked));

                    foreach (var item in ScheduleViewModels)
                        item.IsChecked = value;
                }
            }
        }

        public void LoadSchedules(IEnumerable<ScheduleModel> models)
        {
            ScheduleViewModels.Clear();

            foreach (var model in models)
            {
                ScheduleViewModels.Add(new ScheduleViewModel(model));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
