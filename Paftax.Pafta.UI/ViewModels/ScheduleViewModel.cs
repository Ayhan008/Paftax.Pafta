using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.UI.Models;

namespace Paftax.Pafta.UI.ViewModels
{
    internal partial class ScheduleViewModel(ScheduleModel model) : ObservableObject
    {
        public ScheduleModel Model { get; } = model;

        public long Id => Model.Id;
        public string Name => Model.Name;

        [ObservableProperty]
        private bool isChecked = model.IsChecked;
    }
}
