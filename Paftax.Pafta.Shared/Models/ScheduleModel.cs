using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Interfaces;

namespace Paftax.Pafta.Shared.Models
{
    public partial class ScheduleModel : ObservableObject, IElement
    {
        public required long Id { get; set; }
        public required string Name { get; set; }

        [ObservableProperty]
        private bool isChecked;
    }
}
