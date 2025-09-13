using System.Collections.ObjectModel;
using System.ComponentModel;
using Paftax.Pafta.UI.Models;

namespace Paftax.Pafta.UI.ViewModels
{
    internal class ScheduleViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ScheduleModel> Schedules { get; set; } = [];

        private bool _isChecked;
        public bool IsChecked
        {
            get 
            { 
                return _isChecked; 
            }
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
