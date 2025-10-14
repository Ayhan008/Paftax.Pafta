using CommunityToolkit.Mvvm.ComponentModel;
using Paftax.Pafta.Shared.Interfaces;

namespace Paftax.Pafta.Shared.Models
{
    public partial class FilterModel : ObservableObject, IElement
    {
        public required string Name { get; set; }
        public required long Id { get; set; }
        public bool IsActive { get; set; }
        public int TemplateCount { get; set; }

        [ObservableProperty]
        private bool isChecked;
    }
}
