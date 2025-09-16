using System.Collections.ObjectModel;
using System.ComponentModel;
using Paftax.Pafta.UI.Models;

namespace Paftax.Pafta.UI.ViewModels
{
    internal class ScheduleViewModel(ScheduleModel scheduleModel) : INotifyPropertyChanged
    {
        public long Id
        {
            get { return scheduleModel.Id; }
        }
        public string Name
        {
            get { return scheduleModel.Name; }
        }

        private bool _isChecked = scheduleModel.IsChecked;
        public bool IsChecked
        {
            get { return _isChecked; }

            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsChecked)));
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
