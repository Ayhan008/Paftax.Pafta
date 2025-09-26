using CommunityToolkit.Mvvm.ComponentModel;

namespace Paftax.Pafta.Shared.Models
{
    public partial class ScheduleModel : ObservableObject
    {
        public required long Id { get; set; }
        public required string Name { get; set; }

        [ObservableProperty]
        private bool isChecked;
    }
}
